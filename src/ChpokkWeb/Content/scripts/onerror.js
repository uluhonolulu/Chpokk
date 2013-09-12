
//track errors
window.onerror = function (exception, url, line) {
	var data = { Message: exception, Url: url, LineNumber: line };
	var targetUrl = 'url::ChpokkWeb.Features.CustomerDevelopment.ErrorModel';
	$.post(targetUrl, data);
};

//problem with local storage
var localStorageOk = true;
try {
	var storage = window.localStorage;
} catch(e) {
	localStorageOk = false;
}
if (!localStorageOk) {
	alert("Your browser's security settings are not compatible with some of the Chpokk's features. \r\nPlease run your browser as administrator, or use a different browser.");
}


