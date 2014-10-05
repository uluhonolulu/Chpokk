$(function () {
	window.define = ace.define;
	ace.require("ace/ext/language_tools");
	var editor = ace.edit("ace");
	editor.setTheme("ace/theme/idle_fingers");
	

	//completion
	var codeCompleter = {
		getCompletions: function (editor, session, pos, prefix, callback) {
			if ($('.ace_autocomplete').is(':visible')) { 
				var all = editor.completer.completions ? $.grep(editor.completer.completions.all, function (item) {
					return item.caption.startsWithIgnoreCase(prefix);
				}) : [];
				callback(null, all);
				return;
			}

			var position = getPosition(pos, editor) - 1;
			var text = editor.getValue(); 
			var newChar = text.substr(position, 1);
			var model = tabs.activeModel();
			var editorData = $.extend({}, model, { Content: text, NewChar: newChar, Position: position }); 
			$.post('url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel', editorData, function (data) {
				//if the text changed since then, don't display it (only text before the cursor position matters)
				if (editorTextHasChangedSinceThisCall(pos, editor))
					return;
				var matchingItems = $.grep(data.Items, function(item) {
					return item.Name.startsWithIgnoreCase(prefix);
				});
				var completionData = $.map(matchingItems, function (item, index) {
					return {
						name: item.Name,
						value: item.Name + (item.EntityType == 'Method' ? '()' : ''),
						caption: item.Name,
						className: 'completion_' + item.EntityType + ' completion',
						score: data.Items.length - index
					};//TODO: .completer.insertMatch(this.editor); or if (data.snippet) snippetManager.insertSnippet(this.editor, data.snippet);
				});
				callback(null, completionData);
				//IE fix
				if (editor.completer.popup) {
					var renderer = editor.completer.popup.renderer;
					renderer.desiredHeight = undefined;
					renderer.onResize(true);
					//renderer.$textLayer.checkForSizeChanges();
				}

			});
		}
	};
	// enable autocompletion and snippets
	editor.setOptions({
		enableBasicAutocompletion: true,
		enableSnippets: true
	});
	editor.completers = [codeCompleter];

	// fire autocomplete on any char
	$('#ace').keypress(function (e) {
		if (editor.enableIntellisense) {
			var char = String.fromCharCode(e.which);
			var regexp = /[a-zA-Z_0-9\.]/;
			if (regexp.test(char)) {
				$('#ace').one('keyup', function () {
					editor.commands.exec('startAutocomplete', editor); //temporarily disable the autocomplete
				});
			}
			
			//on enter, check syntax
			if (e.which === $.ui.keyCode.ENTER) {
				checkSyntax(editor);
			}
		}
	});
	
	// enable/disable autocompletion
	$('#autoSuggestOff').click(function() {
		editor.enableIntellisense = !$(this).is(':checked');
	});


	//editor.completers = [snippetCompleter, textCompleter, keyWordCompleter];
});

function editorTextHasChangedSinceThisCall(oldpos, editor) {
	var currentPos = editor.getCursorPosition();
	var currentText = getTextBeforeCursor(currentPos, editor);
	var prevText = getTextBeforeCursor(pos, editor);
	return currentText != prevText;
}

function getPosition(rowColumn, editor) {
	var lines = editor.session.doc.getAllLines();
	var text = editor.getValue();
	var position = 0;
	for (var row = 0; row < rowColumn.row; row++) {
		//we set the position to the start of the (row+1)th line
		//we look for the start of that line, starting from the end of the row-th line
		position = text.indexOf(lines[row + 1], position + lines[row].length);
	}
	position += rowColumn.column;
	return position;
}

//text preceding the cursor position
function getTextBeforeCursor(rowColumn, editor) {
	return editor.getValue().substr(0, getPosition(rowColumn, editor));
}


function loadSelectedFile() {
	var path = jHash.root();
	if (path) {
		var editor = ace.edit("ace");
		loadFile(path, editor, function() {
			selectCode(editor, jHash.val());
		});

	}
}

window.tabs = window.tabs || {
	activeModel: function () {
		if(tabs.activePath) {
			return tabs.all[tabs.activePath].model;
		}
		return window.model;
	}
};
window.tabs.all = window.tabs.all || {};

function loadFile(path, editor, onload) {
	track('loading ' + path);
	$('#fileContent').show();
	var selector = 'li[data-path="' + path.replace(/\\/g, '\\\\') + '"]';
	var itemContainer = $('#solutionBrowser ' + selector + ' .file');
	var fileData = $.extend({}, model, itemContainer.data());
	fileData["ui-draggable"] = null; //fix cyclic error
	// if we haven't loaded this file yet, let's load it and add to cache; then call this method again
	if (!window.tabs.all[path]) {
		track('sending ' + JSON.stringify(fileData));
		$.ajax({
			type: "POST",
			url: 'url::ChpokkWeb.Features.Exploring.FileContentInputModel',
			data: fileData,
			success: function (data) {
				if (!window.tabs.all[path]) {
					var container = $('#navtabs');
					var a = createTab(path, container, editor); // creeate the UI
					window.tabs.all[path] = { model: fileData, content: data.Content, tab: a }; //store the data
					loadFile(path, editor, onload); //call it again so that we use the loaded data					
				}

				function createTab(path, container, editor) {
					var a = $('<a/>').data('toggle', 'tab').data('path', path).attr('title', path).text(path.fileName());
					a.click(function (e) {
						e.preventDefault();
						activateTab(path, editor);
					});
					var li = $('<li/>').append(a);
					container.append(li);
					return a;
				}
			}
		});
		
	} else {
		activateTab(path, editor);
		
		if (onload) {
			onload(editor);
		}
	}
}

function activateTab(path, editor) {
	var activePath = tabs.activePath;
	//store the current content
	if (activePath) {
		tabs.all[activePath].content = editor.getValue();	
	}
	//activate
	var a = tabs.all[path].tab;
	a.tab('show'); //UI
	window.tabs.activePath = path; //should be before setContent, since it uses active tab when parsing 
	//load content into the editor
	setContent(path, editor, tabs.all[path].content);
}

function setContent(path, editor, content) {
	editor.setValue(content, 1);
	//highlighting
	var modelist = ace.require('ace/ext/modelist');
	var mode = modelist.getModeForPath(path).mode;
	editor.getSession().setMode(mode);

	//enable/disable autocompletion
	editor.enableIntellisense = path.endsWith('.cs') || path.endsWith('.vb');
	if (editor.enableIntellisense) {
		checkSyntax(editor);
	}
	//resize -- sorry couldn't do it with CSS
	//$('#codeAndIntelWrapper').height($('#codeAndIntelWrapper').height() - $('#codeAndIntelWrapper')[0].offsetTop);
	//editor.resize();
	
}

function selectCode(editor, selectionData) {
	if (selectionData.line) {
		editor.moveCursorTo(selectionData.line - 1, 0);	
	}


	//var Range = ace.require('ace/range').Range;
	//var range = new Range(3, 4, 3, 6);
	//var marker = editor.getSession().addMarker(range,"ace_selected_word_bro", "text", true);
	//Remove the highlighted word:

	//editor.getSession().removeMarker(marker);
	//Highlight the line:
	//editor.getSession().highlightLines(2, 'ace_invalid');

	//editor.getSession().addMarker( new Range(7, 4, 7, 6), "ace_active_line_yo", "background", true);
	
	//editor.getSession().addMarker(new Range(3, 4, 3, 6), "warning", "line", true);

	//editor.getSession().setAnnotations([{
	//	row: 1,
	//	column: 10,
	//	text: "Strange error",
	//	type: "error" // also warning and information
	//}]);
	
	//var markerId = editor.renderer.addMarker(new Range(1, 10, 1, 15), "warning", "text");

		//This would highlight line 2 (all zero based) column 9-14 by putting a
		//div with the CSS class "warning" below the text. If you want to
		//highlight the full line you have to do:

				//var markerId = editor.renderer.addMarker(new Range(1, 0, 2, 0),
				//"warning", "line");

	//editor.getSession().addGutterDecoration(7, 'error');
//Adds className to the row, to be used for CSS stylings and whatnot.
}

//check syntax for errors
function checkSyntax(editor) {
	var model = tabs.activeModel();
	if (model.PathRelativeToRepositoryRoot) {
		var content = editor.getValue();
		var parseUrl = 'url::ChpokkWeb.Features.Editor.Parsing.ParserInputModel';
		$.post(parseUrl, $.extend(model, { Content: content }), function (data) {
			for (var index in data.Errors) {
				var error = data.Errors[index];
				editor.getSession().setAnnotations([{
					row: error.PositionSpan.StartLinePosition.Line,
					text: error.Message,
					type: "error" // also warning and information
				}]);
			}
		});
	}
}