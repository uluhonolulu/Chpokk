
$(function () {
	amplify.subscribe("suggestion", function (data) {
		var sendUrl = 'url::ChpokkWeb.Features.Demo.SuggestionModel';
		$.post(sendUrl, data, function (response) {
			if (response.StatusCode == 200) {
				suggestionPublished();
			} else {
				suggestionError(response.Message);
			}

		});
	})
});
