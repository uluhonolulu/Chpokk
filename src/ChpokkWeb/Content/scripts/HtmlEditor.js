function HtmlEditor(editorElement) {
    this.editorElement = editorElement;
}

HtmlEditor.prototype.updateHtml = function (callback) {
    var text = this.editorElement.text();
    var toHtmlUrl = "url::ChpokkWeb.Features.Editor.Colorizer.ColorizerInputModel";
    this.editorElement.load(toHtmlUrl, { Code: text }, callback);
};

//HtmlEditor.prototype.initEditor = function () {
//    updateHtml();
//};

HtmlEditor.prototype.colorize = function () {
	var position = this.getCursorPosition();
	//alert(bililiteRange(this.editorElement.get(0))._nativeSelection()[0]);
//	$.gritter.add({ text: bililiteRange(this.editorElement.get(0))._nativeSelection()[0] });
//	var sel = window.getSelection().getRangeAt(0);
//	$.gritter.add({ text: sel.startOffset });
	var self = this;
	this.updateHtml(function () {
		self.setCursorPosition(position);
	});

};

HtmlEditor.prototype.getCursorPosition = function() {
	var range = bililiteRange(this.editorElement.get(0)).bounds('selection');
	return range.bounds()[0];
};

HtmlEditor.prototype.setCursorPosition = function(position) {
	var range = bililiteRange(this.editorElement.get(0)).bounds('selection');
	range.bounds([position, position]).select();
};

//function saveSelection(selection, rootNode) {
//    var caretPosition = selection.anchorOffset;
//    var currentNode = selection.anchorNode;

//    var nodePositions = [];
//    nodePositions.push(caretPosition);
//    while (currentNode.id !== rootNode.id) {
//        nodePositions.push(getNodePosition(currentNode));
//        currentNode = currentNode.parentNode;
//    }
//    return nodePositions;
//}

//function restoreSelection(nodePositions) {
//    var caretPosition = nodePositions[0];
//    var selection = window.getSelection();
//    var range = selection.getRangeAt(0);
//    var currentNode = range.startContainer;
//    for (var i = nodePositions.length - 1; i > 0; i--) {
//        currentNode = currentNode.childNodes[nodePositions[i]];
//    }
//    try {
//        range.setStart(currentNode, caretPosition);
//        selection.removeAllRanges();
//        selection.addRange(range);
//    } catch (e) {
//        debugger;
//        trace(e.toString());
//    }
//}



HtmlEditor.prototype.getDotOffset = function () {
	var sel = window.getSelection().getRangeAt(0);
	sel.setStart(sel.startContainer, sel.startOffset - 1); //so that it selects the last char -- otherwise it becomes a zero rect
	var selRect = sel.getBoundingClientRect();
	var parentBounds = this.editorElement.get(0).getBoundingClientRect();
	var offset = { top: selRect.bottom - parentBounds.top, left: selRect.left - parentBounds.left };
	sel.setStart(sel.startContainer, sel.endOffset); //set the selection back
	return offset;
};

function setEditorHtml(editor, html) {
    var range = bililiteRange(editor.get(0)).bounds('selection'); //
    var position = range.bounds()[0];
    editor.html(html);
    range.bounds([position, position]).select();
   }

function insertHtml(editor, htmlToInsert, position) {
    var range = bililiteRange(editor.get(0)).bounds('selection');
//    var position = range.bounds()[0];
//    console.log("bililite: " + position); //apparently it reports a wrong position in real life
//	console.log("native: " + getCaretPosition(window.getSelection().getRangeAt(0)));
	var fragment = range._nativeRange([0, position]).cloneContents();
	var content = getFragmentSource(fragment);
	//console.log(content);
	var html = editor.html().replace(/&nbsp;/g, ' ');
	html = content + htmlToInsert + html.substring(content.length + 0);
	setEditorHtml(editor, html);
	
}


//function getCaretPosition(editorElement) {
//    var range = bililiteRange(editorElement.get(0)).bounds('selection');
//	return range.bounds()[0];
//}

function getFragmentSource(fragment) {
	var content = '';
	for (var i = 0; i < fragment.childNodes.length; i++) {
		var node = fragment.childNodes[i];
		console.log(node.nodeType);
		if (node.nodeType === 1) {
			content += node.outerHTML;
			console.log(node.outerHTML);
		}
		else {
			content += htmlEncode(node.textContent);
			console.log(node.textContent);
		}

	}
	return content;
}

function getNodePosition(node) {
    var parentNode = node.parentNode;
    if (parentNode === null) return -1;
    return $.inArray(node, parentNode.childNodes);
   }

function htmlEncode(value) {
	return $('<pre/>').text(value).html();
}

function trace(message) {
    $('#log').html($('#log').html() + message + "<br/>\r\n");
}