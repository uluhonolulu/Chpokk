
$(function () {
	amplify.subscribe('ContinuationError', function (continuation) {
		debugger;
		$('#errorContainer').html(continuation.errors[0].message);
		$('.waitContainer').hide();
	});
	$.continuations.bind('AjaxStarted', function () {
		alert('ajax!');
	});

});