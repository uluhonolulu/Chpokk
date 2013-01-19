$(function () {
    $.fn.keyz.keymap.space = $.ui.keyCode.SPACE;
});

function CodeEditor(editorElement, model) {
	//this.editorElement = editorElement
	//TODO: add smoke tests for all keyz
	//debugger;
	var intelContainer = $('[role="intelResults"]');
	intelContainer.hide();
	var htmlEditor = new HtmlEditor(editorElement); 
	var intelManager = new IntelManager(editorElement, intelContainer, htmlEditor, model);
	this.getKeyHandlers = function () {
		return {
			'space': function() {
				htmlEditor.colorize(this);
				return true;
			},

			'.': function() {
				intelManager.showData();
				return true;
			},
			'enter': function() {
				var range = bililiteRange(editorElement.get(0)).bounds('selection');
				range.text('\n', 'end');
				return true;
			},
			'tab': function() {
				range.text('\t &#09;', 'end');
				return true;
			}
		};
	};
    editorElement.keyz({ 'enter': false, 'tab': false }, { 'enter': false }, this.getKeyHandlers());

    new HtmlEditor(editorElement).updateHtml();
}