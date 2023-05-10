// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const _modalPrimaryButtonClick = function () {
    if (!_modalCallback) return;

    _modalCallback($('#modal'));
};

const _closeModalEvent = function () {
    $('#modal').modal('hide');
    $('#modal').modal('dispose');
};

let _modalCallback = undefined;

const generateModalHtmlTemplate = (message) => `
<div class="modal" tabindex="-1" role="dialog" id="modal">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Confirmação</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="_closeModalEvent()">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <p>${message}</p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" onclick="_modalPrimaryButtonClick()">Sim</button>
                <button type="button" class="btn btn-secondary" data-dismiss="modal" onclick="_closeModalEvent()">Não</button>
            </div>
        </div>
    </div>
</div>
`;

function showYesNoConfirmation(message, callback) {
    _modalCallback = callback;
    const modalHtml = generateModalHtmlTemplate(message);
    $('body').append(modalHtml);
    $('#modal').on('hidden.bs.modal', function (e) {
        $('#modal').remove();
    })
    $('#modal').modal('show');
}