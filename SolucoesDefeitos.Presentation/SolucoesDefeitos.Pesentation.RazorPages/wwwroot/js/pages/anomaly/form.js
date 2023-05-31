const form = (function () {
    function _form() {
        const _txtProductSearch = () => $('#txtProductSearch');

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
        };

        return {
            initialize: _initialize
        };
    }

    return new _form();
})();
$(document).ready(() => form.initialize());