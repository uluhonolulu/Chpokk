var dialogOptions = {
	autoOpen: false,
	modal: true,
	width: "auto"
};
var buttons = {
	buttons: {
		"Отмена": function () { $(this).dialog("close"); },
		Ok: function () {
			$("form", $(this)).submit();
			$(this).dialog("close");
		}
	}
};
$("#dialogWrapper, .dialog").each(function () {
	var options = $.extend({}, dialogOptions);
	if (!$(this).hasClass("nobuttons")) {
		options = $.extend(options, buttons);
	}
	$(this).dialog(options);
});

$('#dialogWrapper').dialog("option", "title", "Редактировать поле");
$('#dialogWrapper').dialog('open');