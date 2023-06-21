"use strict";
const list = (function () {
    function _list() {
        const _getComponents = () => {
            return Object.freeze({
                hdnToken: $('input[name="__RequestVerificationToken"]'),
                dvMessages: $('#dvMessages')
            });
        };  

        const _renderErrorServerSide = function (errorMessage) {
            var token = _getComponents().hdnToken.val();
            $.ajax({
                url: "?handler=TempDataError",
                method: "POST",
                data: {
                    __RequestVerificationToken: token,
                    errorMessage
                },
                success: function (data) {
                    _getComponents().dvMessages.html('');
                    _getComponents().dvMessages.html(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error(textStatus);
                    alert('Ocorreu um erro ao processar a mensagem de retorno do servidor.')
                }
            });
        };

        const _deleteRecordCallBack = function (id) {
            return (modalElement) => {
                $.ajax({
                    url: `/api/product/${id}`,
                    type: 'DELETE',
                    success: function () {
                        location.reload();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        let errorMessage = 'Não foi possível excluir o produto.';
                        if (jqXHR.responseJSON
                            && jqXHR.responseJSON.success === false
                            && jqXHR.responseJSON.errors) {
                            errorMessage = jqXHR.responseJSON.errors.join(', ');                            
                        }

                        _renderErrorServerSide(errorMessage);
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