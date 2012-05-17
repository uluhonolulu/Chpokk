
$(function () {
	amplify.subscribe("suggestion", function (data) {
		$.post(sendUrl, data, function (response) {
			if (response.StatusCode == 200) {
				suggestionPublished();
			} else {
				suggestionError(response.Message);
			}

		});
	})
});

