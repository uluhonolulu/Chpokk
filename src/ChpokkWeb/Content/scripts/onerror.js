
//track errors
window.onerror = function (exception, url, line) {
	var data = { Message: exception, Url: url, LineNumber: line };
	var targetUrl = 'url::ChpokkWeb.Features.CustomerDevelopment.ErrorModel';
	$.post(targetUrl, data);
};
//throw new Error("Oh oh, an error has occured");