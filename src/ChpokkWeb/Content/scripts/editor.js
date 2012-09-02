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
    $.fn.keyz.keymap.space = $.ui.keyCode.SPACE;
    var intelManager = new IntelManager($('#code'), $('#intel_results'));

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

function getNodePosition(node) {
    var parentNode = node.parentNode;
    if (parentNode === null) return -1;
    return $.inArray(node, parentNode.childNodes);
}

function getCaretPosition(editableDiv) {
    var caretPos = 0, containerEl = null, sel, range;
    if (window.getSelection) {
        sel = window.getSelection();
        if (sel.rangeCount) {
            range = sel.getRangeAt(0);
            var parentNode = range.commonAncestorContainer.parentNode;
            var grandParentNode = parentNode.parentNode;
            var index = $.inArray(parentNode, grandParentNode.childNodes);
            var length = 0;
            for (var i = 0; i < index; i++) {
                var node = grandParentNode.childNodes[i];
                length += (node.nodeType === 3) ? node.length : node.outerHTML.length;
            }
            caretPos = range.endOffset + length;
            debugger;
            if (parentNode == editableDiv) {
                caretPos = range.endOffset;
            }
        }
    } else if (document.selection && document.selection.createRange) {
        range = document.selection.createRange();
        if (range.parentElement() == editableDiv) {
            var tempEl = document.createElement("span");
            editableDiv.insertBefore(tempEl, editableDiv.firstChild);
            var tempRange = range.duplicate();
            tempRange.moveToElementText(tempEl);
            tempRange.setEndPoint("EndToEnd", range);
            caretPos = tempRange.text.length;
        }
    }
    return caretPos;
}

function trace(message) {
    $('#log').html($('#log').html() + message + "<br/>\r\n");
}