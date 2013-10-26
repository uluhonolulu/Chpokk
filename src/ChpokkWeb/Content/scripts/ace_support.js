$(function () {
	ace.require("ace/ext/language_tools");
	var editor = ace.edit("ace");
	editor.setTheme("ace/theme/clouds");
	var codeCompleter = {
		getCompletions: function (editor, session, pos, prefix, callback) {
			var position = getPosition(pos, editor) - 1;
			var text = editor.getValue(); //text.substr(0,position) + '.' + text.substr(position)
			var newChar = text.substr(position, 1);
			if ($('.ace_autocomplete').is(':visible')) { 
				var all = editor.completer.completions ? $.grep(editor.completer.completions.all, function (item) {
					return item.caption.startsWithIgnoreCase(prefix);
				}) : [];
				callback(null, all);
				return;
			}

			var editorData = $.extend(model, { Text: text, NewChar: newChar, Position: position }); //model + Text, NewChar, Position -- use stackoverflow.com/questions/18013109/retrieve-line-number-of-string-in-ace-editor
			$.post('url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel', editorData, function (data) {
				var completionData = $.map(data.Items, function (item, index) {
					return { name: item.Name, value: item.Name + (item.EntityType == 'Method'? '()' : ''), caption: item.Name, score: data.Items.length - index, meta: item.EntityType };
				});
				callback(null, completionData);
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
		var char = String.fromCharCode(e.which);
		var regexp = /[a-zA-Z_0-9\.]/;
		if (regexp.test(char)) {
			$('#ace').one('keyup', function () {
				//editor.commands.exec('startAutocomplete', editor); //temporarily disable the autocomplete
			});

		}
	});
    

	amplify.subscribe('loadFileRequest', function (data) {
		loadFile(data.path, editor);
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


function loadFile(path, editor) {
    // this is really ugly, since we depend on something we don't see here, but I need to pass the ProjectPath property somehow
    $('#fileContent').show();
	var selector = 'li[data-path="' + path.replace(/\\/g, '\\\\') + '"]';
	var li = $('#solutionBrowser ' + selector);
	var projectPath = (li.length > 0) ? li.data('ProjectPath') : '';
	var fileData = { RepositoryName: model.RepositoryName, PathRelativeToRepositoryRoot: path, ProjectPath: projectPath };
	$.ajax({
		type: "POST",
		url: 'url::ChpokkWeb.Features.Exploring.FileContentInputModel',
		data: fileData,
		success: function (data) {
			editor.setValue(data.Content);
			var mode = path.endsWith('.vb') ? 'vbscript' : 'csharp';
			editor.getSession().setMode("ace/mode/" + mode);
			editor.resize();
			model.PathRelativeToRepositoryRoot = path;
			model.ProjectPath = projectPath;
		}
	});
}