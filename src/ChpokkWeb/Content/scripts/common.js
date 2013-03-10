
$(function () {

	$.continuations.bind('HttpError', function (continuation) {
		var response = continuation.response;
		var message = "Unknown error";
		if (response.getResponseHeader('Content-Type').indexOf('text/html') != -1) {
			message = $(response.responseText).find('i').text();
		}
		if (response.getResponseHeader('Content-Type').indexOf('json') != -1 && continuation.errors && continuation.errors > 0) {
			message = continuation.errors[0].message;
		}

		$.gritter.add({
			title: 'Error!',
			text: message,
			class_name: 'gritter-light'
		});
		
		$('.waitContainer').hide();
	});

});

// util
function currentTime() {
	var dd = new Date();
	var hh = dd.getHours();
	var mm = dd.getMinutes();
	var ss = dd.getSeconds();
	return hh + ":" + mm + ":" + ss;
}


function printStackTrace() {
	var callstack = [];
	var isCallstackPopulated = false;
//	try {
//		i.dont.exist += 0; //doesn't exist- that's the point
//	} catch (e) {
//		if (e.stack) { //Firefox
//			var lines = e.stack.split('\n');
//			for (var i = 0, len = lines.length; i < len; i++) {
//				if (lines[i].match(/^\s*[A-Za-z0-9\-_\$]+\(/)) {
//					callstack.push(lines[i]);
//				}
//			}
//			//Remove call to printStackTrace()
//			callstack.shift();
//			isCallstackPopulated = true;
//		}
//		else if (window.opera && e.message) { //Opera
//			var lines = e.message.split('\n');
//			for (var i = 0, len = lines.length; i < len; i++) {
//				if (lines[i].match(/^\s*[A-Za-z0-9\-_\$]+\(/)) {
//					var entry = lines[i];
//					//Append next line also since it has the file info
//					if (lines[i + 1]) {
//						entry += " at " + lines[i + 1];
//						i++;
//					}
//					callstack.push(entry);
//				}
//			}
//			//Remove call to printStackTrace()
//			callstack.shift();
//			isCallstackPopulated = true;
//		}
//	}
	if (!isCallstackPopulated) { //IE and Safari
		var currentFunction = arguments.callee.caller;
		while (currentFunction) {
			var fname = currentFunction.toString().split('\n')[0];
			callstack.push(fname);
			currentFunction = currentFunction.caller;
		}
	}
	output(callstack);
}

function output(arr) {
	// Output however you want
	alert(arr.join('\n'));
}

window.alert = function (message) {
	$.gritter.add({ text: message });
}