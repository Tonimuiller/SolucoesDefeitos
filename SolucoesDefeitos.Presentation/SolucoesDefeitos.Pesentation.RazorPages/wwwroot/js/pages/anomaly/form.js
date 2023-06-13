const anomalyForm = (function () {
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
                dvImageUpload: $('#dvImageUpload'),
                flImageUpload: $('#flImageUpload'),
                txtImageDescription: $('#txtImageDescription'),                
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

        const _validationRules = {
            addImage: {
                rules: {
                    flImageUpload: 'required',
                    txtImageDescription: 'required'
                },
                messages: {
                    flImageUpload: 'Escolha a imagem',
                    txtImageDescription: 'Informe uma descrição para a imagem'
                }
            }
        };

        const _renderProductTableServerSide = function (products) {
            var token = $('input[name="__RequestVerificationToken"]').val();
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
            _getComponents().dvImageUpload.hide();
            _getComponents().anomalyForm.validate(_validationRules.addImage).destroy();
            _getComponents().flImageUpload.removeClass('error');
            _getComponents().txtImageDescription.removeClass('error');
        };

        const _addImage = function () {
            if (!_getComponents().anomalyForm.valid()) {
                return;
            }
        };

        return {
            initialize: _initialize,
            addProduct: _addProduct,
            deleteProduct: _deleteProduct,
            startAddImage: _startAddImage,
            addImageCancel: _addImageCancel,
            addImage: _addImage
        };
    }

    return new _form();
})();
$(document).ready(() => anomalyForm.initialize());