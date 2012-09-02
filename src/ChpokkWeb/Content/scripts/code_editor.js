$(function () {
    $.fn.keyz.keymap.space = $.ui.keyCode.SPACE;
});

function CodeEditor(element, model) {
    this.element = element;
    this.intelContainer = $('#' + element.data('intelResults'));
    var intelManager = new IntelManager($('#code'), $('#intel_results'));
    $('#code').keyz(null, null, {
        'space': function () {
            colorize(this);
            return true;
        },

        '.': function () {
            intelManager.showData();
            return true;

        }
    });

    updateHtml();
}