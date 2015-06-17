using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.CustomerDevelopment.WhosOnline;
using FubuCore;
using FubuMVC.Core.Http;
using FubuMVC.Core.Http.AspNet;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.CustomerDevelopment {
	public class ActivityTracker: IDisposable {
		private readonly List<ITrack> _log = new List<ITrack>();
		private readonly SmtpClient _mailer;
		private readonly UsageRecorder _usageRecorder;
		private readonly UsageCounter _usageCounter;
		private readonly ISecurityContext _securityContext;
		private readonly WhosOnlineTracker _onlineTracker;
		private ICurrentHttpRequest _httpRequest;
		public ActivityTracker(SmtpClient mailer, UsageRecorder usageRecorder, ISecurityContext securityContext, UsageCounter usageCounter, WhosOnlineTracker onlineTracker, ICurrentHttpRequest httpRequest) {
			_mailer = mailer;
			_usageRecorder = usageRecorder;
			_securityContext = securityContext;
			_usageCounter = usageCounter;
			_onlineTracker = onlineTracker;
			_httpRequest = httpRequest;
		}

		public void Record(string message) {
			Record(null, message, null, null);
		}

		public void Record(string userName, string caption, string url, string browser) {
			if (url.IsEmpty())
				url = _httpRequest.FullUrl();
			_log.Add(new TrackerInputModel{What = caption, Url = url});
			UserName = userName ?? _securityContext.CurrentIdentity.Name;
			if (Browser.IsEmpty()) Browser = browser;
			if (UserName.IsNotEmpty()) {
				_onlineTracker.On(UserName);
			}
		}

		public void Dispose() {
			//if (UserName.IsEmpty()) {
			//	return; //don't send for anonymous
			//}
			if (GetDuration() == TimeSpan.Zero) {
				return; //don't send empty stuff
			}
			try {
				var messageBuilder = new StringBuilder();
				messageBuilder.AppendLine("User: " + (IsLoggedIn? UserName: "anonymous"));
				messageBuilder.AppendLine("Browser: " + Browser);
				foreach (var model in _log) {
					messageBuilder.AppendLine(model.ToString());
				}
				if (_mailer.Host != null) {
					var subject = GetSubject();
					var body = messageBuilder.ToString().Replace(@"\r\n", Environment.NewLine); // making the serialized values readable
					_mailer.Send("actions@chpokk.apphb.com", "uluhonolulu@gmail.com", subject, body);
				}
				if (IsLoggedIn) {
					_usageRecorder.AddUsage(UserName, messageBuilder.ToString(), GetDuration());				
				}

			}
			catch (Exception e) {
				_mailer.Send("actions@chpokk.apphb.com", "uluhonolulu@gmail.com", "Error sending actions: " + e.Message, e.ToString());
			}

			if (IsLoggedIn) {
				_onlineTracker.Off(UserName);			
			}

		}

		private bool IsLoggedIn { get { return UserName.IsNotEmpty(); }}

		private string GetSubject() {
			var subject = "Actions for " + (IsLoggedIn ? UserName : "anonymous");
			if (HasThisAction("Subscribed")) subject += " (SUBSCRIBED!!!)";
			if (HasThisAction("Shit Canceled")) subject += " (canceled!!!)";
			if (_log.OfType<ErrorModel>().Any()) subject += " ERROR!!!";
			if (IsLoggedIn) {
				var previousUsages = _usageCounter.GetUsageCount(UserName);
				subject += ", previous usages: {0}.".ToFormat(previousUsages);				
			}

			var duration = GetDuration();
			subject += " duration:" + duration.ToString(@"h\:mm\:ss");
			if (HasThisAction("createSimpleProjectButton")) subject += " (created a project)";
			if (HasThisAction("http://chpokk.apphb.com/Repository/")) subject += " (opened the editor)";
			return subject;
		}

		private TimeSpan GetDuration() {
			var activeActions = _log.Where(track => !track.ToString().Contains("keepalive")).ToArray();
			if (activeActions.Any()) {
				return activeActions.Last().When - activeActions.First().When;			
			}
			else {
				return TimeSpan.Zero;
			}

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