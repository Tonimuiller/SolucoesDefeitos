"use strict";
const list = (function () {
    function _list() {
        const _getComponents = function () {
            return Object.freeze({
                frmAnomalyList: $("#frmAnomalyList"),
                txtSearchTerm: $("#txtSearchTerm"),
                slManufacturer: $("#slManufacturer"),
                slProductGroup: $("#slProductGroup"),
                slProduct: $("#slProduct")
            });
        }        

        const _slManufacturerOnChange = function (option, checked, select) {
            const manufacturerIds = _getComponents().slManufacturer.val();
            if (!manufacturerIds || !manufacturerIds.length) {
                _getComponents().slProductGroup.multiselect('dataprovider', []);
                _getComponents().slProduct.multiselect('dataprovider', []);
                return;
            }

            const manufacturerIdsQueryParameters = manufacturerIds.map(manufacturerId => `manufacturerIds=${manufacturerId}`);
            $.ajax({
                type: 'GET',
                url: `/api/product-group/anomaly-filter-options?${manufacturerIdsQueryParameters.join('&')}`,
                success: (result, status, xhr) => {
                    const slProductGroupData = result.map(productGroup => {
                        return {
                            label: productGroup.description,
                            title: productGroup.description,
                            value: productGroup.productGroupId
                        };
                    });
                    _getComponents().slProductGroup.multiselect('dataprovider', slProductGroupData);
                    _getComponents().slProduct.multiselect('dataprovider', []);
                },
                error: (xhr, status, error) => {
                    alert('Ocorreu um erro ao carregar as opções de filtro dinamicamente.');
                }
            });
        };

        const _slProductGroupOnChange = function (option, checked, select) {
            const productGroupIds = _getComponents().slProductGroup.val();
            if (!productGroupIds || !productGroupIds.length) {
                _getComponents().slProduct.multiselect('dataprovider', []);
                return;
            }

            const productGroupIdsQueryParameters = productGroupIds.map(productGroupId => `productGroupIds=${productGroupId}`);
            $.ajax({
                type: 'GET',
                url: `/api/product/anomaly-filter-options?${productGroupIdsQueryParameters.join('&')}`,
                success: (result, status, xhr) => {
                    const slProductData = result.map(product => {
                        return {
                            label: product.name,
                            title: product.name,
                            value: product.productId
                        };
                    });
                    _getComponents().slProduct.multiselect('dataprovider', slProductData);
                },
                error: (xhr, status, error) => {
                    alert('Ocorreu um erro ao carregar as opções de filtro dinamicamente.');
                }
            });
        };

        const _buildMultiSelectOptions = function (
            disabledText,
            nonSelectedText,
            onChangeEvent) {
            return Object.freeze({
                onChange: onChangeEvent,
                onSelectAll: onChangeEvent,
                onDeselectAll: onChangeEvent,
                buttonContainer: '<div class="btn-group w-100" />',
                templates: {                    
                    button: '<button type="button" class="multiselect dropdown-toggle btn btn-outline-secondary" data-bs-toggle="dropdown" aria-expanded="false"><span class="multiselect-selected-text"></span></button>',
                    filter: '<div class="multiselect-filter d-flex align-items-center"><i class="fas fa-sm fa-search text-muted"></i><input type="search" class="multiselect-search form-control"  style="margin-left: -0.625rem;"/></div>'
                },
                disableIfEmpty: true,
                disabledText,
                nonSelectedText,
                numberDisplayed: 0,
                includeSelectAllOption: true,
                selectAllText: 'Selecionar todos',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                allSelectedText: 'Todos'
            });
        };

        const _initialize = function () {
            _getComponents().slManufacturer.multiselect(
                _buildMultiSelectOptions(
                    'Nenhum Fabricante disponível',
                    'Selecione um ou mais Fabricante(s)',
                    _slManufacturerOnChange));
            _getComponents().slProductGroup.multiselect(
                _buildMultiSelectOptions(
                    'Nenhum Grupo de Produto disponível',
                    'Selecione um ou mais Grupo(s) de Produtos',
                    _slProductGroupOnChange));
            _getComponents().slProduct.multiselect(
                _buildMultiSelectOptions(
                    'Nenhum Produto disponível',
                    'Selecione um ou mais Produto(s)'));
        };

        const _filterClear = function () {
            _getComponents().txtSearchTerm.val('');
            _getComponents().slManufacturer.multiselect('deselectAll', false);
            _getComponents().slProductGroup.multiselect('deselectAll', false);
            _getComponents().slProduct.multiselect('deselectAll', false);
            _getComponents().slProductGroup.multiselect('dataprovider', []);
            _getComponents().slProduct.multiselect('dataprovider', []);
            _getComponents().frmAnomalyList.submit();
        };

        return {
            initialize: _initialize,
            filterClear: _filterClear
        };
    }

    return new _list();
})();

$(document).ready(function () {
    list.initialize();
});