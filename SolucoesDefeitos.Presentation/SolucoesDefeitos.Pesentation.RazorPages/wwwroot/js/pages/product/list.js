"use strict";
const list = (function () {
    function _list() {

        const _deleteRecordCallBack = function (id) {
            return (modalElement) => {
                $.ajax({
                    url: `/api/product/${id}`,
                    type: 'DELETE',
                    success: function () {
                        location.reload();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {                        
                        alert('Não foi possível excluir o produto.');
                    },
                    complete: function () {
                        modalElement.modal('hide');
                        modalElement.modal('dispose');
                    }
                });
            };
        };

        const _deleteRecord = function (id) {
            showYesNoConfirmation('Deseja realmente excluir o produto?', _deleteRecordCallBack(id));
        };

        return {
            deleteRecord: _deleteRecord
        };

    }

    return new _list();
})();