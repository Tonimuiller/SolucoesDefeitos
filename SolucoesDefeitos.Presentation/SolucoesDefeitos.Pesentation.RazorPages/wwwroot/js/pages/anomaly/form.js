const form = (function () {
    function _form() {
        const _initialize = function () {
            if (!tinymce) {
                console.error("TinyMCE not found.");
                return;
            }

            tinymce.init({
                selector: 'textarea#txtDescription',
                height: 300
            });

            tinymce.init({
                selector: 'textarea#txtRepairSteps',
                height: 600
            });
        };

        return {
            initialize: _initialize
        };
    }

    return new _form();
})();

form.initialize();