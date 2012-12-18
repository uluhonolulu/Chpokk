$(function () {
    $.fn.keyz.keymap.space = $.ui.keyCode.SPACE;
});

function CodeEditor(element, model) {
	//this.element = element
	//TODO: add smoke tests for all keyz
	var intelContainer = $('#' + element.data('intelResults'));
	intelContainer.hide();
	var intelManager = new IntelManager(element, intelContainer, model);
	this.getKeyHandlers = function () {
		return {
			'space': function() {
				colorize(this);
				return true;
			},

			'.': function() {
				intelManager.showData();
				return true;
			},
			'enter': function() {
				var range = bililiteRange(element.get(0)).bounds('selection');
				range.text('\n', 'end');
				return true;
			},
			'tab': function() {
				alert('tab');
				return true;
			}
		};
	};
    element.keyz({ 'enter': false, 'tab': false }, { 'enter': false }, this.getKeyHandlers());

    new HtmlEditor(element).updateHtml();
}