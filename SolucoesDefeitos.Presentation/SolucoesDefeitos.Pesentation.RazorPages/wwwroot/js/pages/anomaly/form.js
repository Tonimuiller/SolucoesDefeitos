﻿const anomalyForm = (function () {
    function _form() {
        const _getComponents = () => {
            return Object.freeze({
                anomalyForm: $('#anomalyForm'),
                hdnAnomalyId: $('#hdnAnomalyId'),
                txtProductSearch: $('#txtProductSearch'),
                hdnSelectedProductId: $('#hdnSelectedProductId'),
                txtProductManufactureYear: $('#txtProductManufactureYear'),
                dvProductTable: $('#dvProductTable'),
                productTableBody: $('#product-table-body'),
                dvAttachments: $('#dvAttachments'),
                dvImageUpload: $('#dvImageUpload'),
                flImageUpload: $('#flImageUpload'),
                txtImageDescription: $('#txtImageDescription'),
                hdnToken: $('input[name="__RequestVerificationToken"]'),
                dvYoutubeVideo: $('#dvYoutubeVideo'),
                txtYoutubeVideoUrl: $('#txtYoutubeVideoUrl'),
                iFrmYoutube: $('#iFrmYoutube'),
                txtYoutubeVideoDescription: $('#txtYoutubeVideoDescription'),
                dvYoutubeVideoPreview: $('#dvYoutubeVideoPreview')
            });
        };        

        const _renderProductAutocompleteItem = (item) => `
            <div>
                <span class="fw-bold text-uppercase fs-4">${item.name}</span><br>
                <span class="fw-bold fs-6">Fabricante: </span><span class="fs-5">${item.manufacturer.name}</span><br>
                <span class="fw-bold fs-6">Grupo: </span><span class="fs-5">${item.productGroup.description}</span><br>
                <span class="fw-bold fs-6">Código: </span><span class="fs-5">${item.code}</span>
            </div>
            `;

        const _validationRules = Object.freeze({
            addImage: {
                rules: {
                    flImageUpload: 'required',
                    txtImageDescription: 'required'
                },
                messages: {
                    flImageUpload: 'Escolha a imagem',
                    txtImageDescription: 'Informe uma descrição para a imagem'
                }
            },
            addYoutubeVideoLink: {
                rules: {
                    txtYoutubeVideoUrl: 'required',
                    txtYoutubeVideoDescription: 'required'
                },
                messages: {
                    txtYoutubeVideoUrl: 'Informe uma url de vídeo do Youtube válida',
                    txtYoutubeVideoDescription: "Informa uma descrição para o vídeo"
                }
            }
        });

        const _attachmentsCategories = Object.freeze({
            PICTURE: 0,
            VIDEO: 1,
            PICTURELINK: 2,
            VIDEOLINK: 3,
            BINARY: 4
        });

        const _renderProductTableServerSide = function (products) {
            var token = _getComponents().hdnToken.val();
            $.ajax({
                url: "?handler=ProductsChange",
                method: "POST",
                data: {
                    __RequestVerificationToken: token,
                    products
                },
                success: function (data) {
                    _getComponents().dvProductTable.html("");
                    _getComponents().dvProductTable.html(data);
                    _getComponents().txtProductSearch.val('');
                    _getComponents().txtProductManufactureYear.val('');
                    _getComponents().hdnSelectedProductId.val(null);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error(textStatus);
                    alert('Ocorreu um erro ao renderizar a tabela de produtos.')
                }
            });
        };

        const _getProductTableDataAsJson = function () {
            const products = [];
            [..._getComponents().productTableBody.children('tr')].forEach((tr, index, children) => {
                if (!$(tr).data("productId")) {
                    return;
                }

                products.push({
                    productId: $(tr).data('productId'),
                    anomalyId: $(tr).data('anomalyId') ?? 0,
                    manufactureYear: $(tr).data('manufactureYear')
                });
            });

            return products;
        };

        const _initialize = function () {
            if (!tinymce) {
                console.error("TinyMCE not found.");
                return;
            }

            tinymce.init({
                selector: 'textarea#txtDescription',
                height: 200
            });

            tinymce.init({
                selector: 'textarea#txtRepairSteps',
                height: 300
            });

            _getComponents().txtProductSearch.autocomplete({
                minLength: 0,
                source: '/api/product/by-term',
                focus: function (event, ui) {
                    _getComponents().txtProductSearch.val(ui.item.name);
                    return false;
                },
                select: function (event, ui) {
                    _getComponents().hdnSelectedProductId.val(ui.item.productId);
                    return false;
                },
                close: function (event, ui) {
                    if (!_getComponents().hdnSelectedProductId.val()) {
                        _getComponents().txtProductSearch.val('');
                    }
                },
                search: function (event, ui) {
                    _getComponents().hdnSelectedProductId.val(null);
                }
            }).autocomplete('instance')._renderItem = function (ul, item) {
                return $('<li>')
                    .addClass('li-divider')
                    .append(_renderProductAutocompleteItem(item))
                    .appendTo(ul);
                };

            _getComponents().txtYoutubeVideoUrl.change(_txtYoutubeVideoUrlChange);
            _getComponents().iFrmYoutube.on('load', _iFrmYoutubeVideoPreviewLoad);
        };

        const _addProduct = function () {
            const productId = _getComponents().hdnSelectedProductId.val()
            if (!productId) {
                alert('Nenhum produto selecionado.');
                return;
            }

            const manufactureYear = _getComponents().txtProductManufactureYear.val();
            if (!manufactureYear || isNaN(manufactureYear)) {
                alert('Informe um ano de fabricação válido.');
                return;
            }

            const products = _getProductTableDataAsJson();
            const existentProduct = products.find((p) => p.productId === productId);
            if (existentProduct) {
                alert('Já existe um item do mesmo produto na lista.');
                return;
            }

            products.push({
                productId,
                manufactureYear,
                anomalyId: _getComponents().hdnAnomalyId.val() ?? 0
            });

            _renderProductTableServerSide(products);
        };

        const _deleteProduct = function (productIndex) {
            showYesNoConfirmation('Deseja realmente excluir o produto?', function (modalElement) {
                modalElement.modal('hide');
                modalElement.modal('dispose');
                const products = _getProductTableDataAsJson();
                if (products[productIndex]) {
                    products.splice(productIndex, 1);
                    _renderProductTableServerSide(products);
                }                
            });
        };

        const _startAddImage = function () {
            _getComponents().dvImageUpload.show();
            _getComponents().anomalyForm.validate(_validationRules.addImage);
        };

        const _addImageCancel = function () {
            _getComponents().flImageUpload.val(null);
            _getComponents().txtImageDescription.val('');
            _getComponents().dvImageUpload.hide();
            _getComponents().anomalyForm.validate(_validationRules.addImage).destroy();
            _getComponents().flImageUpload.removeClass('error');
            _getComponents().txtImageDescription.removeClass('error');
        };

        const _convertBase64 = function (file) {
            return new Promise((resolve, reject) => {
                const fileReader = new FileReader();
                fileReader.readAsDataURL(file);
                fileReader.onload = () => {
                    resolve(fileReader.result);
                };
                fileReader.onerror = (error) => {
                    reject(error);
                };
            });
        };

        const _getAttachmentsAsJson = function () {
            const attachments = [];
            [...$('div[id^=dvAttachment-]')].forEach((attachmentDiv, index) => {
                attachments.push({
                    anomalyId: $(attachmentDiv).data('anomalyId'),
                    attachmentId: $(attachmentDiv).data('attachmentId'),
                    description: $(attachmentDiv).data('attachmentDescription'),
                    storage: $(attachmentDiv).data('attachmentStorage'),
                    category: $(attachmentDiv).data('attachmentCategory'),
                });
            });

            return attachments;
        };

        const _renderAttachmentsServerSide = function (attachments, afterRenderAttachmentsServerSideCallback) {
            var token = _getComponents().hdnToken.val();
            $.ajax({
                url: "?handler=AttachmentsChange",
                method: "POST",
                data: {
                    __RequestVerificationToken: token,
                    attachments
                },
                success: function (data) {
                    _getComponents().dvAttachments.html('');
                    _getComponents().dvAttachments.html(data);
                    if (afterRenderAttachmentsServerSideCallback)
                        afterRenderAttachmentsServerSideCallback();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error(textStatus);
                    alert('Ocorreu um erro ao renderizar a seção de anexos.')
                }
            });
        };

        const _addImage = async function () {
            if (!_getComponents().anomalyForm.valid()) {
                return;
            }

            const imageFile = _getComponents().flImageUpload.prop('files')[0];
            const imageBase64 = await _convertBase64(imageFile);
            const imageDescription = _getComponents().txtImageDescription.val();
            const attachments = _getAttachmentsAsJson();
            attachments.push({
                anomalyId: _getComponents().hdnAnomalyId.val() ?? 0,
                description: imageDescription,
                storage: imageBase64,
                category: _attachmentsCategories.PICTURE
            });

            _renderAttachmentsServerSide(attachments, _addImageCancel);
        };

        const _deleteAttachment = function (attachmentIndex) {
            showYesNoConfirmation('Deseja realmente excluir o anexo?', function (modalElement) {
                modalElement.modal('hide');
                modalElement.modal('dispose');
                const attachments = _getAttachmentsAsJson();
                if (attachments[attachmentIndex]) {
                    attachments.splice(attachmentIndex, 1);
                    _renderAttachmentsServerSide(attachments);
                }
            });
        };

        const _startAddYoutubeVideo = function () {
            _getComponents().dvYoutubeVideo.show();
            _getComponents().anomalyForm.validate(_validationRules.addYoutubeVideoLink);
        };

        const _addYoutubeVideoCancel = function () {
            _getComponents().txtYoutubeVideoUrl.val('');
            _getComponents().iFrmYoutube.attr('src', null);
            _getComponents().dvYoutubeVideoPreview.hide();
            _getComponents().txtYoutubeVideoDescription.val('');
            _getComponents().dvYoutubeVideo.hide();
            _getComponents().anomalyForm.validate(_validationRules.addYoutubeVideoLink).destroy();
            _getComponents().txtYoutubeVideoUrl.removeClass('error');
            _getComponents().txtYoutubeVideoDescription.removeClass('error');
        };

        const _txtYoutubeVideoUrlChange = function () {
            const youtubeVideoUrl = _getComponents().txtYoutubeVideoUrl.val();
            if (!youtubeVideoUrl) {
                _getComponents().iFrmYoutube.attr('src', null);
                _getComponents().dvYoutubeVideoPreview.hide();
                return;
            }

            const youtubeVideoId = _getYoutubeVideoIdFromUrl(youtubeVideoUrl);
            if (!youtubeVideoId) {
                alert('Não foi possível identificar o vídeo.');
                return;
            }

            _getComponents().iFrmYoutube.attr('src',
                `https://www.youtube.com/embed/${youtubeVideoId}?autoplay=0&mute=1`);
        };

        const _getYoutubeVideoIdFromUrl = function (url) {
            // Our regex pattern to look for a youTube ID
            const regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#&?]*).*/;
            //Match the url with the regex
            const match = url.match(regExp);
            //Return the result
            return match && match[2].length === 11 ? match[2] : undefined;
        };

        const _iFrmYoutubeVideoPreviewLoad = function () {
            if (_getComponents().iFrmYoutube.attr('src')) {
                _getComponents().dvYoutubeVideoPreview.show();
            }
        };

        const _addYoutubeVideo = function () {
            if (!_getComponents().anomalyForm.valid()) {
                return;
            }

            const videoId =  _getYoutubeVideoIdFromUrl(_getComponents().txtYoutubeVideoUrl.val());
            const videoDescription = _getComponents().txtYoutubeVideoDescription.val();
            const attachments = _getAttachmentsAsJson();
            attachments.push({
                anomalyId: _getComponents().hdnAnomalyId.val() ?? 0,
                description: videoDescription,
                storage: videoId,
                category: _attachmentsCategories.VIDEOLINK
            });

            _renderAttachmentsServerSide(attachments, _addYoutubeVideoCancel);
        };

        return {
            initialize: _initialize,
            addProduct: _addProduct,
            deleteProduct: _deleteProduct,
            startAddImage: _startAddImage,
            addImageCancel: _addImageCancel,
            addImage: _addImage,
            deleteAttachment: _deleteAttachment,
            startAddYoutubeVideo: _startAddYoutubeVideo,
            addYoutubeVideoCancel: _addYoutubeVideoCancel,
            addYoutubeVideo: _addYoutubeVideo
        };
    }

    return new _form();
})();
$(document).ready(() => anomalyForm.initialize());