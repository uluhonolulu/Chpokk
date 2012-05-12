
$(function () {
	amplify.subscribe("suggestion", function (data) {
		$.post('/demo/send', data, function (response) {
			if (response.StatusCode == 200) {
				suggestionPublished();
			} else {
				suggestionError(response.Message);
			}

		});
	})
});

