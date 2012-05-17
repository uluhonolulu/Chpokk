using System;
using System.Net;
using System.Net.Mail;
using FubuMVC.Core;

namespace ChpokkWeb.Features.Demo {
	public class SuggestController {
		private readonly SmtpClient _mailer;
		public SuggestController(SmtpClient mailer) {
			_mailer = mailer;
		}

		[JsonEndpoint]
		public SuggestionOutputModel Send(SuggestionModel input) {
			var message = "Name: " + input.Name + "\r\n" + "Email: " + input.Email + "\r\n" + "Message: " + input.Message;
			try {
				_mailer.Send("features@chpokk.apphb.com", "uluhonolulu@gmail.com", "Feature suggestion", message);
				return new SuggestionOutputModel { StatusCode = HttpStatusCode.OK };
			}
			catch (Exception exception) {
				return new SuggestionOutputModel{StatusCode = HttpStatusCode.InternalServerError, Message = exception.Message};
			}
		}
	}
}