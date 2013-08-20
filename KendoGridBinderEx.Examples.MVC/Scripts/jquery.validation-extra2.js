function initializeRemotelyValidatingElementsWithAdditionalFields($form) {
    var remotelyValidatingElements = $form.find("[data-val-remote]");

    $.each(remotelyValidatingElements, function (i, element) {
        var $element = $(element);

        var additionalFields = $element.attr("data-val-remote-additionalfields");

        if (additionalFields.length == 0) return;

        var rawFieldNames = additionalFields.split(",");

        var fieldNames = $.map(rawFieldNames, function (fieldName) { return fieldName.replace("*.", ""); });

        $.each(fieldNames, function (i, fieldName) {
            $form.find("#" + fieldName).change(function () {
                 if ($element.is(':enabled')) {
                     // force re-validation to occur
                     $element.removeData("previousValue");
                     $element.valid();
                 }
            });
        });
    });
}