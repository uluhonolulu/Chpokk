﻿(function () {
	if (typeof window.janrain !== 'object') window.janrain = {};
	if (typeof window.janrain.settings !== 'object') window.janrain.settings = {};

	janrain.settings.tokenUrl = new URI("url::ChpokkWeb.Features.Authentication.LoginInputModel").resolve(new URI(location.href)).toString();

	function isReady() { janrain.ready = true; };
	if (document.addEventListener) {
		document.addEventListener("DOMContentLoaded", isReady, false);
	} else {
		window.attachEvent('onload', isReady);
	}

	var e = document.createElement('script');
	e.type = 'text/javascript';
	e.id = 'janrainAuthWidget';

	if (document.location.protocol === 'https:') {
		e.src = 'https://rpxnow.com/js/lib/kopchik/engage.js';
	} else {
		e.src = 'http://widget-cdn.rpxnow.com/js/lib/kopchik/engage.js';
	}

	var s = document.getElementsByTagName('script')[0];
	s.parentNode.insertBefore(e, s);
})();