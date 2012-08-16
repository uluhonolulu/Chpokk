$(function () {

	var listItemTemplate = $.template(null, "<li class='ui-menu-item'><a class='ui-corner-all' nobr><img src=\"/_content/images/Intellisense/Icons.16x16.${EntityName}.png\" />&nbsp;${Text}</a></li>");
	var intelItems = [];
	var filteredItems = [];
	var selectedIntelItem;
	var selectedIntelIndex = -1;
	var currentFilter = "";
	var keyCode = $.ui.keyCode;

	if (BrowserDetect.browser == "Firefox")
		$('#editorDialog textarea').css('padding-left', '3px');

	$('#code').keypress(function (data) {
		if (data.charCode !== null && data.charCode !== 0) {
			var text = data.target.value; // text before keypress
			var newchar = String.fromCharCode(data.charCode);
			if (newchar === "(") return; // sharp bug
			if ($('#results').is(':visible')) {
				currentFilter += newchar;
				filteredItems = $.grep(intelItems, function (item) {
					return item.Text.beginsWith(currentFilter);
				});
				$('#results').empty();
				if (filteredItems.length > 0) {
					$.tmpl(listItemTemplate, filteredItems).appendTo('#results');
					selectedIntelIndex = 0;
					selectedIntelItem = filteredItems[selectedIntelIndex];
					$('#results li a').eq(selectedIntelIndex).addClass("ui-state-hover");
				}
				else {
					selectedIntelIndex = -1;
					selectedIntelItem = null;
					$('#results').hide();
				}
			}
			else {
				if (data.charCode === 46) {
					var shadowText = $(this).caret().replace("<span id='wrapper'>.</span>");
					$('#shadow').html(shadowText);
					var position = $(this).caret().start;
					$('#results').hide();
					var intelUrl = 'url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel';
					var result = $.post(intelUrl, { Text: text, Position: position, NewChar: newchar }, function (inteldata) {
						if (inteldata != null && inteldata.Items != null && inteldata.Items.length > 0) {
							$.each(inteldata.Items, function () { this.EntityName = entityTypes[this.EntityType]; });
							intelItems = inteldata.Items;
							filteredItems = inteldata.Items;
							selectedIntelItem = null;
							selectedIntelIndex = -1;
							$('#results').empty();
							$.tmpl(listItemTemplate, inteldata.Items).appendTo('#results');
							var offset = { top: $('#wrapper').position().top + $('#wrapper').height(), left: $('#wrapper').position().left };
							$('#results').css(offset);
							$('#results').show();

							setEditorPositions();
						}

					});
				}

			}


		}

		updateHtml();
	});

	$('#code').keydown(function (data) {
		if ((data.keyCode === keyCode.SPACE || data.keyCode === keyCode.TAB) && selectedIntelItem !== null) { // space or tab
			data.preventDefault();
			var textToInsert = selectedIntelItem.Text;
			var currentpos = $('#code').caret().end;
			var startpos = currentpos - currentFilter.length;
			$('#code').caret(startpos, currentpos);
			var code = $('#code').caret().replace(textToInsert);
			$('#code').val(code);
			startpos += textToInsert.length;
			$('#code').caret(startpos, startpos);
			$('#results').hide();
			setEditorPositions();
			selectedIntelItem = null;
			selectedIntelIndex = -1;
			currentFilter = "";
		}



		if (data.keyCode === keyCode.DOWN) {
			if (selectedIntelIndex != -1)
				$('#results li a').eq(selectedIntelIndex).removeClass("ui-state-hover");
			selectedIntelIndex += 1;
			selectedIntelItem = filteredItems[selectedIntelIndex];
			$('#results li a').eq(selectedIntelIndex).addClass("ui-state-hover");
			data.preventDefault();
		}
		if (data.keyCode === keyCode.UP && selectedIntelIndex != -1) {
			$('#results li a').eq(selectedIntelIndex).removeClass("ui-state-hover");
			selectedIntelIndex -= 1;
			if (selectedIntelIndex != -1) {
				selectedIntelItem = filteredItems[selectedIntelIndex];
				$('#results li a').eq(selectedIntelIndex).addClass("ui-state-hover");
			}
			else {
				selectedIntelItem = null;
			}
			data.preventDefault();
		}

		if (data.keyCode === keyCode.ESCAPE) { //Esc
			$('#results').hide();
			setEditorPositions();
			selectedIntelItem = null;
			selectedIntelIndex = -1;
			currentFilter = "";
			data.stopPropagation(); //Esc shouldn't close the dialog
		}

		if (data.keyCode === keyCode.ENTER) { //Enter
			$('#html').hide();
		}
	});

	$('#code').keyup(function () { updateHtml(); });


	$('#results li a').live({
		mouseenter:
					function (data) {
						$(this).addClass("ui-state-hover");
						selectedIntelIndex = $(data.target).index('#results li a');
						selectedIntelItem = intelItems[selectedIntelIndex];
					},
		mouseleave:
					function () {
						$(this).removeClass("ui-state-hover");
					},
		click:
					function () {
						var textToInsert = $(this).text().trim();
						var currentpos = $('#code').caret().end;
						var startpos = currentpos - currentFilter.length;
						$('#code').caret(startpos, currentpos);
						var code = $('#code').caret().replace(textToInsert);
						$('#code').val(code);
						startpos += textToInsert.length;
						$('#code').caret(startpos, startpos);
						$('#results').hide();
						selectedIntelItem = null;
						selectedIntelIndex = -1;
						currentFilter = "";

						updateHtml();
						setEditorPositions();
					}
	}
			);

	updateHtml();

});

function setEditorPositions() {
	$('#commoncontainer').css($('#code').position());
	$('#shadow').css($('#code').position());
}

function updateHtml() {
	var text = $('#code')[0].value || $('#code')[0].textContent;
	var toHtmlUrl = 'url::ChpokkWeb.Features.Editor.Colorizer.ColorizerInputModel';
	$('#html').load(toHtmlUrl, { Code: text });
	$('#html').show();
	//setEditorPositions();
}

var entityTypes = [];
entityTypes[0] = 'Class';
entityTypes[1] = 'Field';
entityTypes[2] = 'Property';
entityTypes[3] = 'Method';
entityTypes[4] = 'Event';

function log(msg) {
	$('#log').html($('#log').html() + msg + "<br/>\n");
}

String.prototype.beginsWith = function (t, i) {
	if (i == false) {
		return (t == this.substring(0, t.length));
	}
	else {
		return (t.toLowerCase() == this.substring(0, t.length).toLowerCase());
	}
};

if (typeof String.prototype.trim !== 'function') {
	String.prototype.trim = function () {
		return this.replace(/^\s+|\s+$/g, '');
	};
}