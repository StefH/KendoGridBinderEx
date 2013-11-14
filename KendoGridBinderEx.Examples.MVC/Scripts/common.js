function submitform(frm) {
    if (frm.onsubmit && !frm.onsubmit()) {
        return;
    }
    frm.submit();
}

var _DefaultDateFormat = "MM/dd/yyyy";
var _DefaultDateTemplate = "{0:" + _DefaultDateFormat + "}";