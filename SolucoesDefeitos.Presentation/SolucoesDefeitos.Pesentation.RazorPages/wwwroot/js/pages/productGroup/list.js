const list = (function () {
    function _list() {

        const _deleteRecordCallBack = function (id) {
            return (modalElement) => {
                $.ajax({
                    url: `/api/product-group/${id}`,
                    type: 'DELETE',
                    success: function () {
                        modalElement.modal('hide');
                        modalElement.modal('dispose');
                        location.reload();
                    },
                    error: function () {
                        alert('Não foi possível excluir o Grupo de Produto.');
                    }
                });
            };
        };

        const _deleteRecord = function (id) {
            showYesNoConfirmation('Deseja realmente excluir o Grupo de Produto?', _deleteRecordCallBack(id));
        };

        return {
            deleteRecord: _deleteRecord
        };

    }

    return new _list();
})();