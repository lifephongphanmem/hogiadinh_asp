var ckeditor_data;
var DocumentEditor = function () {
    // Private functions
    var demos = function () {
        DecoupledDocumentEditor
            .create(document.querySelector('.editor'), {
                licenseKey: '',
            })
            .then(editor => {
                //window.editor = editor;
                ckeditor_data = editor;
                // Set a custom container for the toolbar.
                document.querySelector('.document-editor__toolbar').appendChild(editor.ui.view.toolbar.element);
                document.querySelector('.ck-toolbar').classList.add('ck-reset_all');
            })
            .catch(error => {
                console.error('Oops, something went wrong!');
                console.error('Please, report the following error on https://github.com/ckeditor/ckeditor5/issues with the build id and the error stack trace:');
                console.warn('Build id: jqfmsjxun6uc-8xilal3q9c5n');
                console.error(error);
            });
        DecoupledDocumentEditor
            .create(document.querySelector('.editor'), {
                fontSize: {
                    options: [
                        9,
                        11,
                        13,
                        'default',
                        17,
                        19,
                        21
                    ]
                },
                toolbar: [
                    'heading', 'bulletedList', 'numberedList', 'fontSize', 'undo', 'redo'
                ]
            });
            
    }

    return {
        // public functions
        init: function () {
            demos();
        }
    };
}();

// Initialization
jQuery(document).ready(function () {
    DocumentEditor.init();
});