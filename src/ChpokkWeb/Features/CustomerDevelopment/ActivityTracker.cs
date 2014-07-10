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
		private readonly UsageCounter _usageCounter;
		private readonly ISecurityContext _securityContext;
		public ActivityTracker(SmtpClient mailer, UsageRecorder usageRecorder, ISecurityContext securityContext, UsageCounter usageCounter) {
			_mailer = mailer;
			_usageRecorder = usageRecorder;
			_securityContext = securityContext;
			_usageCounter = usageCounter;
		}


		public void Record(string userName, string caption, string url, string browser) {
			_log.Add(new TrackerInputModel{What = caption, Url = url});
			UserName = userName ?? _securityContext.CurrentIdentity.Name;
			if (Browser.IsEmpty()) Browser = browser;
		}

		public void Dispose() {
			if (UserName.IsEmpty()) {
				return; //don't send for anonymous
			}
			var messageBuilder = new StringBuilder();
			messageBuilder.AppendLine("User: " + UserName);
			messageBuilder.AppendLine("Browser: " + Browser);
			foreach (var model in _log) {
				messageBuilder.AppendLine(model.ToString());
			}
			if (_mailer.Host != null) {
				var subject = GetSubject();
				var body = messageBuilder.ToString();
				_mailer.Send("actions@chpokk.apphb.com", "uluhonolulu@gmail.com", subject, body);
			}

			_usageRecorder.AddUsage(UserName, messageBuilder.ToString());
		}

		private string GetSubject() {
			var subject = "Actions for " + UserName;
			var previousUsages = _usageCounter.GetUsageCount(UserName);
			subject += ", previous usages: {0}.".ToFormat(previousUsages);
			var duration = _log.Last().When - _log.First().When;
			subject += " duration:" + duration.ToString();
			if (HasThisAction("error")) subject += " ERROR!!!";
			if (HasThisAction("startTrial")) subject += " (started trial!!!)";
			if (HasThisAction("cancelTrial")) subject += " (canceled trial!!!)";
			if (HasThisAction("createSimpleProjectButton")) subject += " (created a project)";
			if (HasThisAction("http://chpokk.apphb.com/Repository/")) subject += " (opened the editor)";
			return subject;
		}

		private bool HasThisAction(string searchString) {
			return _log.Select(track => track.ToString()).Any(s => s.Contains(searchString));
		}

		public string UserName { get; set; }
		public string Browser { get; set; }
		public void RecordException(ErrorModel model) {
			_log.Add(model);
		}
	}
}