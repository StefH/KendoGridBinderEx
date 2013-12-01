function submitform(frm) {
    if (frm.onsubmit && !frm.onsubmit()) {
        return;
    }
    frm.submit();
}

function guid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

var _DefaultDateFormat = "MM/dd/yyyy";
var _DefaultDateTemplate = "{0:" + _DefaultDateFormat + "}";