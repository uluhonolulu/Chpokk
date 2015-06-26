
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
	
	//track ajax calls
	$(document).ajaxSend(function (event, jqxhr, settings) {
		track("AJAX: " + settings.url);
	});

	//track button clicks
	$(document).on('click', '.btn, a, input:checkbox, input:radio, #templates span.file', function (e) {
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
	$.ajax(url, {
		type: "POST",
		data: data,
		global: false
	});		
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

//use data in selectors
(function ($) {
	var _dataFn = $.fn.data;
	$.fn.data = function (key, val) {
		if (typeof val !== 'undefined') {
			$.expr.attrHandle[key] = function (elem) {
				return $(elem).attr(key) || $(elem).data(key);
			};
		}
		return _dataFn.apply(this, arguments);
	};
})(jQuery);

//another version of the above
(function () {

	var matcher = /\s*(?:((?:(?:\\\.|[^.,])+\.?)+)\s*([!~><=]=|[><])\s*("|')?((?:\\\3|.)*?)\3|(.+?))\s*(?:,|$)/g;

	function resolve(element, data) {

		data = data.match(/(?:\\\.|[^.])+(?=\.|$)/g);

		var cur = jQuery.data(element)[data.shift()];

		while (cur && data[0]) {
			cur = cur[data.shift()];
		}

		return cur || undefined;

	}

	jQuery.expr[':'].data = function (el, i, match) {

		matcher.lastIndex = 0;

		var expr = match[3],
			m,
			check, val,
			allMatch = null,
			foundMatch = false;

		while (m = matcher.exec(expr)) {

			check = m[4];
			val = resolve(el, m[1] || m[5]);

			switch (m[2]) {
				case '==': foundMatch = val == check; break;
				case '!=': foundMatch = val != check; break;
				case '<=': foundMatch = val <= check; break;
				case '>=': foundMatch = val >= check; break;
				case '~=': foundMatch = RegExp(check).test(val); break;
				case '>': foundMatch = val > check; break;
				case '<': foundMatch = val < check; break;
				default: if (m[5]) foundMatch = !!val;
			}

			allMatch = allMatch === null ? foundMatch : allMatch && foundMatch;

		}

		return allMatch;

	};

}());

//format
String.prototype.toFormat = function(values) {
	var result = this;
	for (var prop in values) {
		result = result.replace('{' + prop + '}', values[prop]);
	}
	return result;
};


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

String.prototype.contains = function (substr) {
	return this.indexOf(substr) >= 0;
};

String.prototype.startsWithIgnoreCase = function(prefix) {
	return this.toLowerCase().startsWith(prefix.toLowerCase());
};

String.prototype.parentFolder = function() {
	var lastSlash = this.lastIndexOf('\\');
	return this.substring(0, lastSlash);
};

String.prototype.fileName = function() {
	var lastSlash = this.lastIndexOf('\\');
	return this.substring(lastSlash + 1);
};

String.prototype.htmlEncode = function() {
	return $('<div/>').text(this).html();
};

String.prototype.htmlDecode = function() {
	return $('<div/>').html(this).text();
};

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
