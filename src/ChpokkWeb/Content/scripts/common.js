
$(function () {
	amplify.subscribe('ContinuationError', function (continuation) {
		alert(continuation.errors[0].message);
	});
});