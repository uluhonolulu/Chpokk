function updateHtml(callback) {
    var text = $('#code').text();
    var toHtmlUrl = "url::ChpokkWeb.Features.Editor.Colorizer.ColorizerInputModel";
    $('#code').load(toHtmlUrl, { Code: text }, callback);
}

function initEditor() {
    updateHtml();
}



function colorize(editor) {
        var range = bililiteRange(editor).bounds('selection'); //
        var position = range.bounds()[0];
        updateHtml(function () {
        	range.bounds([position, position]).select();
        });
    
}

function saveSelection(selection, rootNode) {
    var caretPosition = selection.anchorOffset;
    var currentNode = selection.anchorNode;

    var nodePositions = [];
    nodePositions.push(caretPosition);
    while (currentNode.id !== rootNode.id) {
        nodePositions.push(getNodePosition(currentNode));
        currentNode = currentNode.parentNode;
    }
    return nodePositions;
}

function restoreSelection(nodePositions) {
    var caretPosition = nodePositions[0];
    var selection = window.getSelection();
    var range = selection.getRangeAt(0);
    var currentNode = range.startContainer;
    for (var i = nodePositions.length - 1; i > 0; i--) {
        currentNode = currentNode.childNodes[nodePositions[i]];
    }
    try {
        range.setStart(currentNode, caretPosition);
        selection.removeAllRanges();
        selection.addRange(range);
    } catch (e) {
        debugger;
        trace(e.toString());
    }
      }

function wrapTheDot(editor) {
	var wrappedDot = '<span id=\'wrapper\'>.</span>';
	var range = bililiteRange(editor.get(0)).bounds('selection');//
	var position = range.bounds()[0];
	range.bounds([0, position - 1]);
	var fragment = range._nativeRange(range.bounds()).cloneContents();
	var content = getFragmentSource(fragment);
	//position = bililiteRange(editor.get(0)).bounds('selection').bounds()[0];
	var html = editor.html().replace(/&nbsp;/g, ' ');
	html = content + wrappedDot + html.substring(content.length + 1);
	html = html.replace(/<span id="wrapper">\.<\/span>/gi, '.');
	setEditorHtml(editor, html);
}

function setEditorHtml(editor, html) {
    var range = bililiteRange(editor.get(0)).bounds('selection'); //
    var position = range.bounds()[0];
    editor.html(html);
    range.bounds([position, position]).select();
   }

function insertHtml(editor, htmlToInsert, position) {
    var range = bililiteRange(editor.get(0)).bounds('selection');
//    var position = range.bounds()[0];
    console.log("bililite: " + position); //apparently it reports a wrong position in real life
	console.log("native: " + getCaretPosition(window.getSelection().getRangeAt(0)));
	var fragment = range._nativeRange([0, position]).cloneContents();
	var content = getFragmentSource(fragment);
	//console.log(content);
	var html = editor.html().replace(/&nbsp;/g, ' ');
	html = content + htmlToInsert + html.substring(content.length + 0);
	setEditorHtml(editor, html);
	
}

function setCaretPosition(editor, position) {
    var range = bililiteRange(editor.get(0)).bounds('selection');
    range.bounds([position, position]);
	range.select();
}

//function getCaretPosition(editor) {
//    var range = bililiteRange(editor.get(0)).bounds('selection');
//	return range.bounds()[0];
//}

function getFragmentSource(fragment) {
	var content = '';
	for (var i = 0; i < fragment.childNodes.length; i++) {
		var node = fragment.childNodes[i];
		if (node.nodeType === 1) {
			content += node.outerHTML;
		}
		else {
			content += node.textContent;
		}

	}
	return content;
}

function getNodePosition(node) {
    var parentNode = node.parentNode;
    if (parentNode === null) return -1;
    return $.inArray(node, parentNode.childNodes);
}

function trace(message) {
    $('#log').html($('#log').html() + message + "<br/>\r\n");
}