$(document).ready(function () {
    var $validator = null;

    $('#document-verifier-submit').on('reset', function () {
        $validator.resetForm();
        resetFiles('#FormFile');
    });


    $.fn.filepond.registerPlugin(FilePondPluginFileValidateSize);
    $.fn.filepond.registerPlugin(FilePondPluginFileValidateType);

    const validFiles = ['image/jpeg',
        'image/webp',
        'image/png',
        'application/pdf',
    ];

    $('#FormFile').filepond({
        allowMultiple: false,
        storeAsFile: true,
        maxFileSize: '5MB',
        acceptedFileTypes: validFiles,
        credits: false,
        required:true
    });

    $('#btnLegalBack').on('click', function () {
        $.LoadingOverlay("show");
        window.location.href = "/Home/ListDocuments";
    });       


    $('#btnValidateBack').on('click', function () {
        $.LoadingOverlay("show");
        window.location.href = "/Validate/ListDocuments";
    });       

    $('#FormFile').change(function () {
        //fix for filepond issue : required validation does not disapper on file selection below code will remove required validation error message once file is selected
        var $filepondid = $(this).find('.filepond--browser').attr('id');
        if ($filepondid.length != 0) {
            $('#' + $filepondid + '-error').css('display', 'none');
            $(this).find('.filepond--browser').removeClass('error');
        }
    });

    function resetFiles($element) {
        if ($element == null)
            return;

        FilePond.destroy(document.querySelector($element));
        FilePond.create(document.querySelector($element));
    }

    $validator = $("#document-verifier-submit").validate({

        submitHandler: function (form) {
            $.LoadingOverlay("show");
            form.submit();
        }
    });
});