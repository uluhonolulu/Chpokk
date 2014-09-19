
//track errors
//window.onerror = function (exception, url, line) {
//	var data = { Message: exception, Url: url, LineNumber: line, StackTrace: getStackTrace().join('\n') };
//	reportException(data);
//};

//bind continuation errors to alerts
$.continuations.bind('HttpError', function (continuation) {
    var response = continuation.response;
    var message;
    if (response.getResponseHeader('Content-Type') && response.getResponseHeader('Content-Type').indexOf('text/html') != -1) {
        message = $(response.responseText).find('i').text();
        if (!message) {
            message = $(response.responseText).filter('title').text();
        }
    }
    if (response.getResponseHeader('Content-Type') && response.getResponseHeader('Content-Type').indexOf('json') != -1 && continuation.errors && continuation.errors.length > 0) {
        message = continuation.errors[0].message;
    }

    if (message) danger(message);

    $('.waitContainer').hide();
    $('.modal').modal('hide');
});


function reportException(data) {
	var targetUrl = 'url::ChpokkWeb.Features.CustomerDevelopment.ErrorModel';
	$.post(targetUrl, data);	
}

function reportErrorObject(e) {
	var stackTrace;
	try {
		stackTrace = getStackForErrorObject(e);
	} catch(ee) {
	} 
	try {
		if (!stackTrace) stackTrace = getStackTrace();
	} catch(ee) {
	} 
	if (stackTrace) stackTrace = stackTrace.join('\n');
	var data = { Message: e.message, StackTrace: stackTrace };
	reportException(data);
}

function getStackForErrorObject(e) {
	var callstack = [];
	if (e.stack) { //Firefox
		var lines = e.stack.split('\n');
		for (var i = 0, len = lines.length; i < len; i++) {
			if (lines[i].match(/^[A-Za-z0-9\-_\$\s]+\(/)) {
				callstack.push(lines[i]);
			}
		}
		//Remove call to printStackTrace()
		callstack.shift();
		
	}
	else if (window.opera && e.message) { //Opera
		var lines = e.message.split('\n');
		for (var i = 0, len = lines.length; i < len; i++) {
			if (lines[i].match(/^\s*[A-Za-z0-9\-_\$]+\(/)) {
				var entry = lines[i];
				//Append next line also since it has the file info
				if (lines[i + 1]) {
					entry += " at " + lines[i + 1];
					i++;
				}
				callstack.push(entry);
			}
		}
		//Remove call to printStackTrace()
		callstack.shift();
	}	
}

function getStackTrace() {
	var callstack = [];
	var isCallstackPopulated = false;
	try {
		i.dont.exist += 0; //doesn't exist- that's the point
	} catch (e) {
		if (e.stack) { //Firefox
			var lines = e.stack.split('\n');
			for (var i = 0, len = lines.length; i < len; i++) {
				if (lines[i].match(/^[A-Za-z0-9\-_\$\s]+\(/)) {
					callstack.push(lines[i]);
				}
			}
			//Remove call to printStackTrace()
			callstack.shift();
			if (callstack.length > 1) { //IE would put here just the onerror function
				isCallstackPopulated = true;
			}
			
		}
		else if (window.opera && e.message) { //Opera
			var lines = e.message.split('\n');
			for (var i = 0, len = lines.length; i < len; i++) {
				if (lines[i].match(/^\s*[A-Za-z0-9\-_\$]+\(/)) {
					var entry = lines[i];
					//Append next line also since it has the file info
					if (lines[i + 1]) {
						entry += " at " + lines[i + 1];
						i++;
					}
					callstack.push(entry);
				}
			}
			//Remove call to printStackTrace()
			callstack.shift();
			isCallstackPopulated = true;
		}
	}
	if (!isCallstackPopulated) { //IE and Safari
		var currentFunction = arguments.callee.caller;
		while (currentFunction) {
			var fname = currentFunction.toString().split('\n')[0];
			callstack.push(fname);
		    try {
				currentFunction = currentFunction.caller;     
		    } catch(e) {
		        callstack.push(e.message || e);
		        currentFunction = null;
		    } 
		}
	}
	return callstack;
}

//problem with local storage
var localStorageOk = true;
try {
	var storage = window.localStorage;
} catch(e) {
	localStorageOk = false;
}
if (!localStorageOk) {
	alert("Your browser's security settings are not compatible with some of the Chpokk's features. \r\nPlease run your browser as administrator, or use a different browser.");
	$(function() {
		throw new Error("Problem accessing window.localStorage");
	});
	
}

//window.onerror = function (errorMsg, url, lineNumber, column, errorObj) {
//	var data = {
//		Message: 'Error: ' + errorMsg + ' Script: ' + url + ' Line: ' + lineNumber
//			+ ' Column: ' + column + ' StackTrace: ' + errorObj, StackTrace: 'none'
//	};
//	reportException(data);
//};

//on AJAX error, track
$(document).ajaxError(function (event, jqxhr, settings, exception) {
	var data = { event: event, request: jqxhr, settings: settings, exception: exception };
	track("AJAX Error: " + JSON.stringify(data));
});
