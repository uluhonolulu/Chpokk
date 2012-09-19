$(function () {
    $.fn.keyz.keymap.space = $.ui.keyCode.SPACE;
});

function CodeEditor(element, model) {
    //this.element = element;
	var intelContainer = $('#' + element.data('intelResults'));
	intelContainer.hide();
    var intelManager = new IntelManager(element, intelContainer, model);
    element.keyz({ 'enter': false }, { 'enter': false }, {
    	'space': function () {
    		colorize(this);
    		return true;
    	},

    	'.': function () {
    		intelManager.showData();
    		wrapTheDot(element);
    		var offset = { top: $('#wrapper').position().top + $('#wrapper').height(), left: $('#wrapper').position().left };
    		intelContainer.css(offset);
    		return true;
    	},
    	'enter': function () {
    		var range = bililiteRange(element.get(0)).bounds('selection');
    		range.text('\n', 'end');
    		return true;
    	}
    });

    updateHtml();
}