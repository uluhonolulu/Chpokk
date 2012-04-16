
$(function () {
	amplify.subscribe("suggestion", function (data) {
		$.post('/demo/send', data, function (response) {
			//alert(response.StatusCode);
			if (response.StatusCode == 200) {
				$('#suggestDialog').modal('hide');
				$('#thanks_for_suggestion').dialog('open');
			} else {
				$('#suggestionError').show();
				$('#suggestionErrorText').text(response.Message);
			}

		});
	})
});