"use strict";
const list = (function () {
    function _list() {
        const _getComponents = function () {
            return Object.freeze({
                txtSearchTerm: $("#txtSearchTerm"),
                slManufacturer: $("#slManufacturer"),
                slProductGroup: $("#slProductGroup"),
                slProduct: $("#slProduct")
            });
        }

        const _initialize = function () {
            _getComponents().slManufacturer.multiselect({
                templates: {
                    button: '<button type="button" class="multiselect dropdown-toggle btn btn-primary" data-bs-toggle="dropdown" aria-expanded="false"><span class="multiselect-selected-text"></span></button>',
                    filter: '<div class="multiselect-filter d-flex align-items-center"><i class="fas fa-sm fa-search text-muted"></i><input type="search" class="multiselect-search form-control"  style="margin-left: -0.625rem;"/></div>'
                },
                disableIfEmpty: true,
                disabledText: 'Nenhum Fabricante disponível',
                nonSelectedText: 'Selecione um ou mais Fabricante(s)',
                numberDisplayed: 0,
                includeSelectAllOption: true,
                selectAllText: 'Selecionar todos',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
            _getComponents().slProductGroup.multiselect({
                templates: {
                    button: '<button type="button" class="multiselect dropdown-toggle btn btn-primary" data-bs-toggle="dropdown" aria-expanded="false"><span class="multiselect-selected-text"></span></button>',
                    filter: '<div class="multiselect-filter d-flex align-items-center"><i class="fas fa-sm fa-search text-muted"></i><input type="search" class="multiselect-search form-control" /></div>'
                },
                disableIfEmpty: true,
                disabledText: 'Nenhum Grupo de Produto disponível',
                nonSelectedText: 'Selecione um ou mais Grupo(s) de Produtos',
                numberDisplayed: 0,
                includeSelectAllOption: true,
                selectAllText: 'Selecionar todos',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
            _getComponents().slProduct.multiselect({
                templates: {
                    button: '<button type="button" class="multiselect dropdown-toggle btn btn-primary" data-bs-toggle="dropdown" aria-expanded="false"><span class="multiselect-selected-text"></span></button>',
                    filter: '<div class="multiselect-filter d-flex align-items-center"><i class="fas fa-sm fa-search text-muted"></i><input type="search" class="multiselect-search form-control" /></div>'
                },
                disableIfEmpty: true,
                disabledText: 'Nenhum Produto disponível',
                nonSelectedText: 'Selecione um ou mais Produto(s)',
                numberDisplayed: 0,
                includeSelectAllOption: true,
                selectAllText: 'Selecionar todos',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
        };

        return {
            initialize: _initialize
        };
    }

    return new _list();
})();

$(document).ready(function () {
    list.initialize();
});