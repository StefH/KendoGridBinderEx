function submitform(frm) {
    if (frm.onsubmit && !frm.onsubmit()) {
        return;
    }
    frm.submit();
}