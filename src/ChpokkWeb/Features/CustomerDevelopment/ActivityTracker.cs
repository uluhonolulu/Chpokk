using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using FubuCore;

namespace ChpokkWeb.Features.CustomerDevelopment {
	public class ActivityTracker: IDisposable {
		private readonly List<ClickTrackerInputModel> _log = new List<ClickTrackerInputModel>();
		private readonly SmtpClient _mailer;
		public ActivityTracker(SmtpClient mailer) {
			_mailer = mailer;
		}


		public void Record(string userName, string caption, string url) {
			_log.Add(new ClickTrackerInputModel{ButtonName = caption, Url = url});
			if (userName != null) {
				UserName = userName;
			}
		}

		public void Dispose() {
			var messageBuilder = new StringBuilder();
			string userName = UserName?? "anonymous";
			messageBuilder.AppendLine("User: " + userName);
			foreach (var model in _log) {
				messageBuilder.AppendLine("Url: {0}, button: {1}".ToFormat(model.Url, model.ButtonName));
			}
			if (_mailer.Host != null) _mailer.Send("actions@chpokk.apphb.com", "uluhonolulu@gmail.com", "Actions for " + userName, messageBuilder.ToString());

		}

		public string UserName { get; set; }
	}
}