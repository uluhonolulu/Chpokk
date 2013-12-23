using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using ChpokkWeb.Features.Authentication;
using FubuCore;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.CustomerDevelopment {
	public class ActivityTracker: IDisposable {
		private readonly List<ITrack> _log = new List<ITrack>();
		private readonly SmtpClient _mailer;
		private readonly UsageRecorder _usageRecorder;
		private readonly ISecurityContext _securityContext;
		public ActivityTracker(SmtpClient mailer, UsageRecorder usageRecorder, ISecurityContext securityContext) {
			_mailer = mailer;
			_usageRecorder = usageRecorder;
			_securityContext = securityContext;
		}


		public void Record(string userName, string caption, string url) {
			_log.Add(new ClickTrackerInputModel{ButtonName = caption, Url = url});
			UserName = userName ?? _securityContext.CurrentIdentity.Name;
		}

		public void Dispose() {
			if (UserName.IsEmpty()) {
				return; //don't send for anonymous
			}
			var messageBuilder = new StringBuilder();
			messageBuilder.AppendLine("User: " + UserName);
			foreach (var model in _log) {
				messageBuilder.AppendLine(model.ToString());
			}
			if (_mailer.Host != null) {
				var subject = "Actions for " + UserName;
				var body = messageBuilder.ToString();
				if (body.Contains("startTrial")) {
					subject += " (started trial!!!)";
				}
				if (body.Contains("cancelTrial")) {
					subject += " (canceled trial!!!)";
				}
				_mailer.Send("actions@chpokk.apphb.com", "uluhonolulu@gmail.com", subject, body);
			}

			_usageRecorder.AddUsage(UserName, messageBuilder.ToString());
		}

		public string UserName { get; set; }
		public void RecordException(ErrorModel model) {
			_log.Add(model);
		}
	}
}