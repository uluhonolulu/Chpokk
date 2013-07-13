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
			'space': function () {
				htmlEditor.colorize(this);
				return true;
			},

			'.': function () {
				intelManager.showData();
				return true;
			},
			'enter': function () {
				htmlEditor.onEnter();
				return true;
			},
			'tab': function () {
				var range = bililiteRange(editorElement.get(0)).bounds('selection');
				range.text('	', 'end');
				return true;
			}
		};
	};
    editorElement.keyz({ 'enter': false, 'tab': false }, { 'enter': false }, this.getKeyHandlers());

    new HtmlEditor(editorElement).updateHtml();

    //Keep Alive
    var keepAlive = function (interval) {
    	$.get('/editor/keepalive');
    	window.setTimeout(function () { keepAlive(interval); }, interval);
    };

    keepAlive(20 * 1000);
	
}