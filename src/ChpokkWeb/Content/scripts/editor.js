function updateHtml(callback) {
    var text = $('#code').text();
    var toHtmlUrl = "url::ChpokkWeb.Features.Editor.Colorizer.ColorizerInputModel";
    $('#code').load(toHtmlUrl, { Code: text }, callback);
}

function initEditor() {
    updateHtml();
    // CodeEditor has $('#code')
    // and model: RepositoryName, ProjectPath
    // model is initialized in _editor.spark
    // and CodeEditor too
}



function colorize(editor) {
        var selection = window.getSelection();
        var nodePositions = saveSelection(selection, editor);
        //debugger;
        updateHtml(function () {
            restoreSelection(nodePositions);
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
	var html = editor.html();
	var position = bililiteRange(editor.get(0)).bounds('selection').bounds()[0];
	html = html.substring(0, position - 1) + '<span id=\'wrapper\'>.</span>' + html.substring(position);
	editor.html(html);
}

function getNodePosition(node) {
    var parentNode = node.parentNode;
    if (parentNode === null) return -1;
    return $.inArray(node, parentNode.childNodes);
}

function trace(message) {
    $('#log').html($('#log').html() + message + "<br/>\r\n");
}