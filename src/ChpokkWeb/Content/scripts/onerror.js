
//track errors
window.onerror = function (exception, url, line) {
	var data = { Message: exception, Url: url, LineNumber: line, StackTrace: getStackTrace().join('\n') };
	var targetUrl = 'url::ChpokkWeb.Features.CustomerDevelopment.ErrorModel';
	$.post(targetUrl, data);
};


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
			isCallstackPopulated = true;
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
			currentFunction = currentFunction.caller;
		}
	}
	return callstack;
}

function output(arr) {
	// Output however you want
	alert(arr.join('\n'));
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


