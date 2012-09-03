$(function () {
    $.fn.keyz.keymap.space = $.ui.keyCode.SPACE;
});

function CodeEditor(element, model) {
    //this.element = element;
    var intelContainer = $('#' + element.data('intelResults'));
    var intelManager = new IntelManager(element, intelContainer, model);
    element.keyz(null, null, {
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