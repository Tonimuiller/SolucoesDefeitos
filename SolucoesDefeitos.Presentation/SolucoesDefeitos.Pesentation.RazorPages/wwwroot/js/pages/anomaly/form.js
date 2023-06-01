const anomalyForm = (function () {
    function _form() {
        const _txtProductSearch = () => $('#txtProductSearch');
        const _txtProductManufactureYear = () => $('#txtProductManufactureYear');
        const _productTableBody = $('#tbl-product-body');
        const _selectedProduct = () => {
            return {
                id: $('#hdnSelectedProductId'),
                name: $('#hdnSelectedProductName'),
                manufacturer: $('#hdnSelectedProductManufacturer'),
                group: $('#hdnSelectedProductGroup'),
                code: $('#hdnSelectedProductCode'),
                manufactureYear: _txtProductManufactureYear()
            };
        };

        const _renderProductAutocompleteItem = (item) => `
            <div>
                <span class="fw-bold text-uppercase fs-4">${item.name}</span><br>
                <span class="fw-bold fs-6">Fabricante: </span><span class="fs-5">${item.manufacturer.name}</span><br>
                <span class="fw-bold fs-6">Grupo: </span><span class="fs-5">${item.productGroup.description}</span><br>
                <span class="fw-bold fs-6">Código: </span><span class="fs-5">${item.code}</span>
            </div>
            `;

        const _renderProductTableRow = (product) => `
            <tr id="tr-product-${product.id}">
                <th scope="row">
                    <input type="hidden" name="Anomaly.ProductSpecifications.Index" value="${product.id}" />
                    <input type="hidden" name="Anomaly.ProductSpecifications[${product.id}].ProductId" value="${product.id}" />
                    <button type="button" class="btn btn-danger" onclick="anomalyForm.deleteProduct(${product.id})">Excluir</button>
                </th>
                <td>                                
                    <span>${product.name}</span>
                </td>
                <td>
                    <span>${product.manufacturer}</span>
                </td>
                <td>
                    <input type="hidden" name="Anomaly.ProductSpecifications[${product.id}].ManufactureYear" value="${product.manufactureYear}" />
                    <span>${product.manufactureYear}</span>
                </td>
                <td>${product.code}</td>
                <td>${product.group}</td>
            </tr>
        `;

        const _renderProductEmptyTableRow = () => `
            <tr id="tr-product-norecord">
                <td colspan="6" class="text-center">Nenhum produto/equipamento especificado</td>
            </tr>
        `;

        const _setSelectedProduct = (item) => {
            _selectedProduct().id.val(item?.productId);
            _selectedProduct().name.val(item?.name);
            _selectedProduct().manufacturer.val(item?.manufacturer.name);
            _selectedProduct().group.val(item?.productGroup.description);
            _selectedProduct().code.val(item?.code);
        };

        const _getSelectedProduct = () => {
            if (!_selectedProduct().id.val())
                return null;
            return {
                id: _selectedProduct().id.val(),
                name: _selectedProduct().name.val(),
                manufacturer: _selectedProduct().manufacturer.val(),
                group: _selectedProduct().group.val(),
                code: _selectedProduct().code.val(),
                manufactureYear: _selectedProduct().manufactureYear.val()
            };
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

            _txtProductSearch().autocomplete({
                minLength: 0,
                source: '/api/product/by-term',
                focus: function (event, ui) {
                    _txtProductSearch().val(ui.item.name);
                    return false;
                },
                select: function (event, ui) {
                    _setSelectedProduct(ui.item);
                    return false;
                },
                close: function (event, ui) {
                    if (!_selectedProduct().id.val()) {
                        _txtProductSearch().val('');
                    }
                },
                search: function (event, ui) {
                    _setSelectedProduct(null);
                }
            }).autocomplete('instance')._renderItem = function (ul, item) {
                return $('<li>')
                    .addClass('li-divider')
                    .append(_renderProductAutocompleteItem(item))
                    .appendTo(ul);
            };
        };

        const _addProduct = function () {
            const product = _getSelectedProduct();
            if (!product) {
                alert('Nenhum produto selecionado.');
                return;
            }

            const manufactureYear = _txtProductManufactureYear().val();
            if (!manufactureYear || isNaN(manufactureYear)) {
                alert('Informe um ano de fabricação válido.');
                return;
            }

            const productRowHtml = _renderProductTableRow(product);
            _productTableBody.append(productRowHtml);

            if ($('#tr-product-norecord').length) {
                $('#tr-product-norecord').remove();
            }

            _txtProductSearch().val('');
            _txtProductManufactureYear().val('');
            _setSelectedProduct(null);
        };

        const _deleteProduct = function (productId) {
            showYesNoConfirmation('Deseja realmente excluir o produto?', function (modalElement) {
                const row = $(`#tr-product-${productId}`);
                if (!row) {
                    return;
                }

                row.remove();
                
                if (!_productTableBody.find('tr').length) {
                    _productTableBody.append(_renderProductEmptyTableRow());
                }

                modalElement.modal('hide');
                modalElement.modal('dispose');
            });
        };

        return {
            initialize: _initialize,
            addProduct: _addProduct,
            deleteProduct: _deleteProduct
        };
    }

    return new _form();
})();
$(document).ready(() => anomalyForm.initialize());