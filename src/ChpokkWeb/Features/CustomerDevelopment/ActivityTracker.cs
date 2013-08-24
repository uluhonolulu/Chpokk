using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using FubuMVC.Core.Security;
using FubuCore;

namespace ChpokkWeb.Features.CustomerDevelopment {
	public class ActivityTracker: IDisposable {
		private readonly List<ClickTrackerInputModel> _log = new List<ClickTrackerInputModel>();
		private readonly ISecurityContext _securityContext;
		private readonly SmtpClient _mailer;
		public ActivityTracker(SmtpClient mailer, ISecurityContext securityContext) {
			_mailer = mailer;
			_securityContext = securityContext;
		}


		public void Record(ClickTrackerInputModel model) {
			_log.Add(model);
		}

		public void Dispose() {
			var messageBuilder = new StringBuilder();
			string userName = _securityContext.IsAuthenticated() ? _securityContext.CurrentIdentity.Name : "anonymous";
			messageBuilder.AppendLine("User: " + userName);
			foreach (var model in _log) {
				messageBuilder.AppendLine("Url: {0}, button: {1}".ToFormat(model.Url, model.ButtonName));
			}
			if (_mailer.Host != null) _mailer.Send("actions@chpokk.apphb.com", "uluhonolulu@gmail.com", "Actions for " + userName, messageBuilder.ToString());

		}
	}
}