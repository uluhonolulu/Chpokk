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
	var preFragment = range._nativeRange([0, position - 1]).cloneContents();
	var endPosition = range.bounds('end').bounds()[1];
	var postFragment = range._nativeRange([position, endPosition]).cloneContents();
	editor.empty();
	editor.get(0).appendChild(preFragment); // or fragment.cloneNode(true)
	var dotNode = $(wrappedDot).get(0);
	editor.get(0).appendChild(dotNode);
	editor.get(0).appendChild(postFragment);
    range.bounds([position, position]).select();
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
//    console.log("bililite: " + position); //apparently it reports a wrong position in real life
//	console.log("native: " + getCaretPosition(window.getSelection().getRangeAt(0)));
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