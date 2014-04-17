
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



    //traditional ajaxsettings so that we don't screw up with arrays
    $.ajaxSetup({ traditional: true });

	//track button clicks
	$(document).on('click', '.btn, a, input:checkbox', function (e) {
		var button = e.target;
		var buttonId = $(button).text().trim() + ' (' + button.id + ')';
		track(buttonId);
	});
	
	//track janrain cancel
	$(document).on('click', '#janrainModal > img', function (e) {
		track("Cancel login");
	});
	
	//track page load
	$(window).on('load', function() {
		track('Page load');
	});

});

//track client-side stuff
function track(message) {
	var browser = BrowserDetect.browser + ' ' + BrowserDetect.version + ' on ' + BrowserDetect.OS;
	var data = { What: message, Url: window.location.toString(), Browser: browser };
	var url = 'url::ChpokkWeb.Features.CustomerDevelopment.TrackerInputModel';
	$.post(url, data);		
}

(function() {
	var alertTemplate = '<div class="alert alert-dismissable alert-${type}" style=\'white-space:pre;\'> <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>${message}</div>';
	function getAlertFunction(type) {
	    return function (message) {
	        var messageDiv = $.tmpl(alertTemplate, { type: type, message: message });
			messageDiv.appendTo($('#alertContainer'));
	        var messageDivHtml = messageDiv.get(0);
	        if (messageDivHtml.scrollWidth > messageDivHtml.clientWidth) {
	            messageDiv.css('white-space', 'normal').css('text-align', 'left');
	        }
		};
	}

	window.danger = getAlertFunction('danger');
	window.success = getAlertFunction('success');
	window.info = getAlertFunction('info');
}());

window.alert = danger;
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

String.prototype.startsWith = function(prefix) {
	return this.indexOf(prefix) === 0;
};

String.prototype.startsWithIgnoreCase = function (prefix) {
	return this.toLowerCase().startsWith(prefix.toLowerCase());
}

//serializing form data for submitting
$.fn.serializeObject = function () {
	var o = {};
	var a = this.serializeArray();
	$.each(a, function () {
		if (o[this.name] !== undefined) {
			if (!o[this.name].push) {
				o[this.name] = [o[this.name]];
			}
			o[this.name].push(this.value || '');
		} else {
			o[this.name] = this.value || '';
		}
	});
	// if model is defined, let's combine
	if (model)
		o = $.extend(model, o);
	//return JSON.stringify(o);
    return o;
};
