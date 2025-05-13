$(document).ready(function () {
    var $validator = null;

    $('#document-verifier-submit').on('reset', function () {
        $validator.resetForm();
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
        credits: false
    });

    $('#btnLegalBack').on('click', function () {
        $.LoadingOverlay("show");
        window.location.href = "/Home/ListDocuments";
    });       

    $validator = $("#document-verifier-submit").validate({
        submitHandler: function (form) {
            $.LoadingOverlay("show");
            form.submit();
        }
    });
});