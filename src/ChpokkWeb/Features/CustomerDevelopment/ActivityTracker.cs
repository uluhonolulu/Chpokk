using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.CustomerDevelopment.WhosOnline;
using FubuCore;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.CustomerDevelopment {
	public class ActivityTracker: IDisposable {
		private readonly List<ITrack> _log = new List<ITrack>();
		private readonly SmtpClient _mailer;
		private readonly UsageRecorder _usageRecorder;
		private readonly UsageCounter _usageCounter;
		private readonly ISecurityContext _securityContext;
		private readonly WhosOnlineTracker _onlineTracker;
		public ActivityTracker(SmtpClient mailer, UsageRecorder usageRecorder, ISecurityContext securityContext, UsageCounter usageCounter, WhosOnlineTracker onlineTracker) {
			_mailer = mailer;
			_usageRecorder = usageRecorder;
			_securityContext = securityContext;
			_usageCounter = usageCounter;
			_onlineTracker = onlineTracker;
		}


		public void Record(string userName, string caption, string url, string browser) {
			_log.Add(new TrackerInputModel{What = caption, Url = url});
			UserName = userName ?? _securityContext.CurrentIdentity.Name;
			if (Browser.IsEmpty()) Browser = browser;
			if (UserName.IsNotEmpty()) {
				_onlineTracker.On(UserName);
			}
		}

		public void Dispose() {
			if (UserName.IsEmpty()) {
				return; //don't send for anonymous
			}
			try {
				var messageBuilder = new StringBuilder();
				messageBuilder.AppendLine("User: " + UserName);
				messageBuilder.AppendLine("Browser: " + Browser);
				foreach (var model in _log) {
					messageBuilder.AppendLine(model.ToString());
				}
					var subject = GetSubject();
				if (_mailer.Host != null) {
					var body = messageBuilder.ToString();
					_mailer.Send("actions@chpokk.apphb.com", "uluhonolulu@gmail.com", subject, body);
				}
				_usageRecorder.AddUsage(UserName, messageBuilder.ToString(), GetDuration());
			}
			catch (Exception e) {
				_mailer.Send("actions@chpokk.apphb.com", "uluhonolulu@gmail.com", "Error sending actions: " + e.Message, e.ToString());
			}


			_onlineTracker.Off(UserName);
		}

		private string GetSubject() {
			var subject = "Actions for " + UserName;
			if (_log.OfType<ErrorModel>().Any()) subject += " ERROR!!!";
			var previousUsages = _usageCounter.GetUsageCount(UserName);
			subject += ", previous usages: {0}.".ToFormat(previousUsages);
			var duration = GetDuration();
			subject += " duration:" + duration.ToString(@"h\:mm\:ss");
			if (HasThisAction("startTrial")) subject += " (started trial!!!)";
			if (HasThisAction("cancelTrial")) subject += " (canceled trial!!!)";
			if (HasThisAction("createSimpleProjectButton")) subject += " (created a project)";
			if (HasThisAction("http://chpokk.apphb.com/Repository/")) subject += " (opened the editor)";
			return subject;
		}

		private TimeSpan GetDuration() {
			return _log.Last().When - _log.First().When;
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