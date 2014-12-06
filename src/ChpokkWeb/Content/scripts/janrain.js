(function () {
	if (typeof window.janrain !== 'object') window.janrain = {};
	if (typeof window.janrain.settings !== 'object') window.janrain.settings = {};

	janrain.settings.tokenUrl = new URI("url::ChpokkWeb.Features.Authentication.LoginInputModel").resolve(new URI(location.href)).toString();
	janrain.settings.actionText = "Sign in using";
	janrain.settings.tokenAction = 'event';
	//janrain.engage.signin.appendTokenParams({ 'param1': 'value1', 'param2': 'value2', 'param3': 'value3' });
    
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
	//overwite the Janrain's method to avoid the exception
	//TODO: handle the unhappy path (e.g. Cancel or invalid credentials)
	janrain.apps.engage.signin.loginHandler = function (response) {
		$.getScript(response.redirectUrl);// get and execute the janrain script containing the code that invokes the onProviderLoginToken handler(s)
		//if (response.stat === 'ok') {
		//	//if(response.welcome_info_name) display it
		//	$.post('/authentication/login', { token: response.authenticity_token });
		//}
		//debugger;
		//actually leave it empty and use 
		//                a ? ('ok' === a.stat && ('event' === b || 'hybrid' === b || 'noRedirect' === b ? ('undefined' != typeof storage && storage.set('janrainEngageAuthenticityToken', a.authenticity_token), 'noRedirect' !== b && W(a.redirectUrl))  : (b = document.createElement('form'), b.action = a.redirectUrl, b.method = 'POST', janrain.settings.targetTop && (b.target = '_top'), document.body.appendChild(b), b.submit())), 'fail' === a.stat && (a.err && 160 === a.err.code ? (janrain.events.onProviderLoginCancel.fire(a), 'undefined' !== typeof h && h.refresh && h.refresh())  : janrain.events.onProviderLoginError.fire(a), 'undefined' !== typeof jb && clearTimeout(jb), janrain.events.onShareLoginCancel && 'fail' === a.stat && janrain.events.onShareLoginCancel.fire(a)))
		// {"stat":"ok","redirectUrl":"http://kopchik.rpxnow.com/no_redirect?loc=f69fa124584a3a80adcc51afae9e0457e43038a5","origin":null,"widget_type":"auth","provider":"google","welcome_info_name":"Artem Smirnov","authenticity_token":"e5f099f0da29bd4b6b3a25fb2702a7c015d849c7"}
		//console.log('loginHandler');
		//console.log(JSON.stringify(response));
	};
	//janrain.events.onProviderLoginStart.addHandler(function() {
	//	console.log('Login Start!');
	//});
	//janrain.events.onProviderLoginComplete.addHandler(function(response) {
	//	console.log('Login complete!');
	//	console.log(JSON.stringify(response));
	//});
	//janrain.events.onProviderLoginError.addHandler(function(response) {
	//	console.log('Login Error!');
	//	console.log('response.err.code = ' + response.err.code);
	//	console.log('response.err.msg = ' + response.err.msg);
	//	console.log('response.origin = ' + response.origin);
	//	console.log('response.state = ' + response.state);
	//});
	//janrain.events.onProviderLoginSuccess.addHandler(function (response) {
	//	console.log('Login Succcess!');
	//	console.log(JSON.stringify(response));
	//});
	//janrain.events.onReturnExperienceFound.addHandler(function(response) {
	//	console.log('Return Experience Found!');
	//	console.log('response.name = ' + response.name);
	//	console.log('response.returnProvider = ' + response.returnProvider);
	//	console.log('response.welcomeName = ' + response.welcomeName);
	//});
	janrain.events.onProviderLoginToken.addHandler(function(response) {
		//console.log('Provider Login Token Returned!');
		//console.log(JSON.stringify(response));
		$.post('/authentication/login', { token: response.token, ConnectionId: model.ConnectionId });
		clearLog();
	});
}

function loginLog(message) {
	$('.janrainContent').append($('<div/>').text(message));
}

function clearLog() {
	$('.janrainContent').empty();
}
$(function() {
	$.connection.simpleLoggerHub.client.log = loginLog;
});

