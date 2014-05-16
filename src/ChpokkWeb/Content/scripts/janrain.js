(function () {
	if (typeof window.janrain !== 'object') window.janrain = {};
	if (typeof window.janrain.settings !== 'object') window.janrain.settings = {};

	janrain.settings.tokenUrl = new URI("url::ChpokkWeb.Features.Authentication.LoginInputModel").resolve(new URI(location.href)).toString();
	janrain.settings.actionText = "Sign in using";
    
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


function janrainWidgetOnload() {
	janrain.events.onProviderLoginStart.addHandler(function() {
		console.log('Login Start!');
	});
	janrain.events.onProviderLoginComplete.addHandler(function(response) {
		console.log('Login complete!');
		console.log('response.provider = ' + response.provider);
	});
	janrain.events.onProviderLoginError.addHandler(function(response) {
		console.log('Login Error!');
		console.log('response.err.code = ' + response.err.code);
		console.log('response.err.msg = ' + response.err.msg);
		console.log('response.origin = ' + response.origin);
		console.log('response.state = ' + response.state);
	});
	janrain.events.onProviderLoginSuccess.addHandler(function(something) {
		console.log('Login Succcess!');
	});
	janrain.events.onReturnExperienceFound.addHandler(function(response) {
		console.log('Return Experience Found!');
		console.log('response.name = ' + response.name);
		console.log('response.returnProvider = ' + response.returnProvider);
		console.log('response.welcomeName = ' + response.welcomeName);
	});
	janrain.events.onProviderLoginToken.addHandler(function(response) {
		console.log('Provider Login Token Returned!');
		console.log('response.token = ' + response.token);
	});
}