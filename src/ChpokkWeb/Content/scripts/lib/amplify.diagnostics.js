amplifyDiagnostics = (function () {
	var messages = [];
	var publish = amplify.publish;
	amplify.publish = function () {
		var msg = {
			topic: arguments[0]
		};

		if (arguments.length != 1) {
			msg.data = arguments[1];
		}

		messages.push(msg);
		publish.apply(this, arguments);
	};

	var module = {
		display: function () {
			var diagnostics = $('<div id="diagnostics-display" class="diagnostics"><h2>Amplify Messages</h2><ul class="messages"></ul></div>');
			diagnostics.css({
				left: '4%',
				width: '90%',
				top: '5%',
				padding: '10px',
				color: 'white',
				position: 'fixed',
				opacity: .90,
				'border-radius': '10px',
				background: '#222 none repeat scroll 0'
			});
			_.each(messages, function (msg) {
				var item = $('<li style="margin-bottom:10px;list-style-type:none"></li>');

				item.append('<span class="topic" style="font-weight:bold; margin-right:10px;display:block;">' + msg.topic + '</span>');
				item.append('<span class="data">' + JSON.stringify(msg.data) + '</span>');

				item.appendTo($('ul', diagnostics));
			});

			diagnostics.appendTo('body');
			diagnostics.show();
		},
		hide: function () {
			$('#diagnostics-display').remove();
		}
	};

	$(document).bind('keyup', function (e) {
		if (e.keyCode === 191 && !$(e.target).is(":input")) {
			module.display();
			return;
		}

		if (e.keyCode === 27 && !$(e.target).is(":input")) {
			module.hide();
		}
	});


	return module;
} ());