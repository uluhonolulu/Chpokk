$(function () {
	ace.require("ace/ext/language_tools");
	var editor = ace.edit("ace");
	editor.setTheme("ace/theme/clouds");
	editor.getSession().setMode("ace/mode/csharp"); //TODO: set language on load
	var codeCompleter = {
		getCompletions: function (editor, session, pos, prefix, callback) {
			var position = getPosition(pos, editor) - 1;
			var text = editor.getValue(); //text.substr(0,position) + '.' + text.substr(position)
			var newChar = text.substr(position, 1);
			if (newChar !== '.') { //autocomplete only on dot
				callback(null, []);
				return;
			}
			//.ace_autocomplete -- popup
			var editorData = $.extend(model, { Text: text, NewChar: newChar, Position: position }); //model + Text, NewChar, Position -- use stackoverflow.com/questions/18013109/retrieve-line-number-of-string-in-ace-editor
			$.post('url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel', editorData, function (data) {
				var completionData = $.map(data.Items, function (item, index) {
					return { name: item.Name, value: item.Name, score: data.Items.length - index, meta: 'code' };
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
	editor.completers = [codeCompleter, editor.completers[2]];

	// fire autocomplete on any char
	editor.on('change', function (e) {
		var eventData = e.data;
		if (eventData.action === 'insertText' && eventData.text.length === 1) {
			var regexp = /[a-zA-Z_0-9\.]/;
			if (regexp.test(eventData.text)) {
				//editor.commands.exec('startAutocomplete', editor);
			}
		}
	});

	$('#ace').keypress(function (e) {
		var char = String.fromCharCode(e.which);
		var regexp = /[a-zA-Z_0-9\.]/;
		if (regexp.test(char)) {
			editor.commands.exec('startAutocomplete', editor);
		}
	});

	amplify.subscribe('loadFileRequest', function (data) {
		loadFile(data.path, editor);
	});
	//editor.completers = [snippetCompleter, textCompleter, keyWordCompleter];
	//gathering and sorting: 1054
	//editor/intellisense/getintellisensedata
});

function getPosition(rowColumn, editor) {
	var lines = editor.session.doc.getAllLines();
	var position = 0;
	for (var row = 0; row < rowColumn.row; row++) {
		position += lines[row].length + 1;
	}
	position += rowColumn.column;
	return position;
}

function loadFile(path, editor) {
	// this is really ugly, since we depend on something we don't see here, but I need to pass the ProjectPath property somehow
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
			editor.resize();
			model.PathRelativeToRepositoryRoot = path;
			model.ProjectPath = projectPath;
		}
	});
}