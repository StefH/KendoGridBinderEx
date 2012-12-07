(function ($) {
    $.validator.setDefaults({
        onkeyup: function (element) {
            if ($(element).attr('data-val-remote-url')) {
                return false;
            }
            else {
                $(element).validate();
                return $(element).valid();
            }
        }
    })
}(jQuery));