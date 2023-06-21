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
                },
            });
            _getComponents().slProductGroup.multiselect({
                templates: {
                    button: '<button type="button" class="multiselect dropdown-toggle btn btn-primary" data-bs-toggle="dropdown" aria-expanded="false"><span class="multiselect-selected-text"></span></button>',
                },
            });
            _getComponents().slProduct.multiselect({
                templates: {
                    button: '<button type="button" class="multiselect dropdown-toggle btn btn-primary" data-bs-toggle="dropdown" aria-expanded="false"><span class="multiselect-selected-text"></span></button>',
                },
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