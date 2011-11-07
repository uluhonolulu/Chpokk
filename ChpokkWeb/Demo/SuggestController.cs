using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using FubuMVC.Core;

namespace ChpokkWeb.Demo {
	public class SuggestController {
		private readonly SmtpClient _mailer;
		public SuggestController(SmtpClient mailer) {
			_mailer = mailer;
		}

		[JsonEndpoint]
		public SuggestionOutputModel Send(SuggestionModel input) {
			var message = "Name: " + input.Name + "\r\n" + "Email: " + input.Email + "\r\n" + "Message: " + input.Message;
			new SmtpClient().Send("features@chpokk.apphb.com", "uluhonolulu@gmail.com", "Feature suggestion", message);
			return new SuggestionOutputModel{StatusCode = HttpStatusCode.OK};
		}
	}

	public class SuggestionOutputModel {
		public HttpStatusCode StatusCode { get; set; }
		public string Message { get; set; }
	}

	public class SuggestionModel {
		public string Message { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
	}
}