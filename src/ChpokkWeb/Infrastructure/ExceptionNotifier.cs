using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ChpokkWeb.Infrastructure {
	public class ExceptionNotifier {
		private SmtpClient _mailer;
		public ExceptionNotifier(SmtpClient mailer) {
			_mailer = mailer;
		}

		public void Notify(Exception exception) {
			if (_mailer.Host != null) {
				Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
			}
		}
	}
}