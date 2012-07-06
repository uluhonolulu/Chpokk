
$(function () {
	amplify.subscribe('ContinuationError', function (continuation) {
		debugger;
		$('#errorContainer').html(continuation.errors[0].message);
		$('.waitContainer').hide();
	});
	$.continuations.bind('HttpError', function (continuation) {
		$.gritter.add({
			// (string | mandatory) the heading of the notification
			title: 'Error!',
			// (string | mandatory) the text inside the notification
			text: 'This will fade out after a certain amount of time. '
		});
		$('.waitContainer').hide();
	});

});