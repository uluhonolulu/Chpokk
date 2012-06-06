
$(function () {
	amplify.subscribe('ContinuationError', function (continuation) {
		$('#errorContainer').html(continuation.errors[0].message);
		$('.waitContainer').hide();
	});
});