﻿$(function () {
    window.define = ace.define;
    ace.require("ace/ext/language_tools");
	var editor = ace.edit("ace");
	editor.setTheme("ace/theme/idle_fingers");
    
    //bug: .ace_content too narrow in IE
    
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
			var editorData = $.extend({}, model, { Content: text, NewChar: newChar, Position: position }); 
			$.post('url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel', editorData, function (data) {
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
        }
	});        

    
	//editor.completers = [snippetCompleter, textCompleter, keyWordCompleter];
});

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


function loadSelectedFile() {
	var path = jHash.root();
	if (path) {
		var editor = ace.edit("ace");
		loadFile(path, editor, function() {
			selectCode(editor, jHash.val());
		});

	}
}

function loadFile(path, editor, onload) {
    // this is really ugly, since we depend on something we don't see here, but I need to pass the ProjectPath property somehow
    $('#fileContent').show();
	var selector = 'li[data-path="' + path.replace(/\\/g, '\\\\') + '"]';
	var itemContainer = $('#solutionBrowser ' + selector + ' .file');
	var fileData = $.extend({}, model, itemContainer.data());
	$.ajax({
		type: "POST",
		url: 'url::ChpokkWeb.Features.Exploring.FileContentInputModel',
		data: fileData,
		success: function (data) {
		    editor.setValue(data.Content, 1);
		    
            //highlighting
			var modelist = ace.require('ace/ext/modelist');
			var mode = modelist.getModeForPath(path).mode;
			editor.getSession().setMode(mode);
		    
		    //enable/disable autocompletion
		    editor.enableIntellisense = path.endsWith('.cs') || path.endsWith('.vb');
			editor.resize();
			$.extend(model, itemContainer.data()); //TODO: multitab support

			if (onload) {
				onload(editor);
			}
		}
	});
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

	editor.getSession().addGutterDecoration(7, 'error');
//Adds className to the row, to be used for CSS stylings and whatnot.
}