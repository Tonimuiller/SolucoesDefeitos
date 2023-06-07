const viewPage = (function () {
    function _viewPage() {

        const _initialize = function () {
        };

        const _deleteCallBack = function (id) {
            return (modalElement) => {
                $.ajax({
                    url: `/api/anomaly/${id}`,
                    type: 'DELETE',
                    success: function () {
                        modalElement.modal('hide');
                        modalElement.modal('dispose');
                        window.location.href = '/Anomaly/List';
                    },
                    error: function () {
                        alert('Não foi possível excluir a Solução e Defeito.');
                    }
                });
            };
        };

        const _delete = function (id) {
            showYesNoConfirmation('Deseja realmente excluir a Solução e Defeito?', _deleteCallBack(id));
        };

        return {
            initialize: _initialize,
            delete: _delete
        };
    };

    return new _viewPage();
})();
$(document).ready(function () {
    viewPage.initialize();
});