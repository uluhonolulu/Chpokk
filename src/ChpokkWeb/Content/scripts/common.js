
$(function () {

	//popovers
	$('.bootstrap-popover').each(function () {
		var content = function () {
			var contentSelector = $(this).attr('popover') || this.hash;
			var placement = $(contentSelector).attr('placement') || 'right';
			return $(contentSelector).html();
		};
		$(this).popover({ content: content, html: true });
	});


	//bind continuation errors to alerts
	$.continuations.bind('HttpError', function (continuation) {
		var response = continuation.response;
		var message;
		if (response.getResponseHeader('Content-Type') && response.getResponseHeader('Content-Type').indexOf('text/html') != -1) {
			message = $(response.responseText).find('i').text();
		}
		if (response.getResponseHeader('Content-Type') && response.getResponseHeader('Content-Type').indexOf('json') != -1 && continuation.errors && continuation.errors > 0) {
			message = continuation.errors[0].message;
		}

		if(message) danger(message);

		$('.waitContainer').hide();
	});

	//track button clicks
	$(document).on('click', '.btn, a', function (e) {
		var button = e.target;
		var buttonId = button.id || $(button).text();
		var data = { ButtonName: buttonId, Url: window.location.toString() };
		var url = 'url::ChpokkWeb.Features.CustomerDevelopment.ClickTrackerInputModel';
		$.post(url, data);
	});
	
	//track janrain cancel
	$(document).on('click', '#janrainModal > img', function (e) {
		var data = { ButtonName: "Cancel login", Url: window.location.toString() };
		var url = 'url::ChpokkWeb.Features.CustomerDevelopment.ClickTrackerInputModel';
		$.post(url, data);
	});

});

(function() {
	var alertTemplate = '<div class="alert alert-dismissable alert-${type}" style=\'white-space:pre;\'> <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>${message}</div>';
	function getAlertFunction(type) {
		return function(message) {
			$.tmpl(alertTemplate, {type:type, message: message}).appendTo($('#alertContainer'));
		};
	}

	window.danger = getAlertFunction('danger');
	window.success = getAlertFunction('success');
	window.info = getAlertFunction('info');
}());

// util
function currentTime() {
	var dd = new Date();
	var hh = dd.getHours();
	var mm = dd.getMinutes();
	var ss = dd.getSeconds();
	return hh + ":" + mm + ":" + ss;
}

String.prototype.endsWith = function (suffix) {
	return this.indexOf(suffix, this.length - suffix.length) !== -1;
};



window.alert = danger;