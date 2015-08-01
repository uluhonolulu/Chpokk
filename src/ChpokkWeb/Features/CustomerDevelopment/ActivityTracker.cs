﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.CustomerDevelopment.WhosOnline;
using ChpokkWeb.Features.ProjectManagement.AddSimpleProject;
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
		private IEnumerable<IAmImportant> _importantRules;
		public ActivityTracker(SmtpClient mailer, UsageRecorder usageRecorder, ISecurityContext securityContext, UsageCounter usageCounter, WhosOnlineTracker onlineTracker, ICurrentHttpRequest httpRequest, IEnumerable<IAmImportant> importantRules) {
			_mailer = mailer;
			_usageRecorder = usageRecorder;
			_securityContext = securityContext;
			_usageCounter = usageCounter;
			_onlineTracker = onlineTracker;
			_httpRequest = httpRequest;
			_importantRules = importantRules;
		}

		public void Record(string message) {
			Record(null, message, null, null);
		}

		public void Record(string userName, string caption, string url, string browser) {
			//if (url.IsEmpty())
			//	url = _httpRequest.FullUrl();
			_log.Add(new TrackerInputModel{What = caption, Url = url});
			if (userName.IsNotEmpty()) {
				UserName = userName;
			}
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
				messageBuilder.AppendLine("");
				foreach (var rule in _importantRules) {
					var message = rule.GetMessage(_log.Cast<TrackerInputModel>());
					if (message != null) {
						messageBuilder.AppendLine(message);
					}
				}
				messageBuilder.AppendLine("");
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

	public interface IAmImportant {
		string GetMessage(IEnumerable<TrackerInputModel> log);
	}

	public class DurationRule : IAmImportant {
		public string GetMessage(IEnumerable<TrackerInputModel> log) {
			var startLoginRecord = log.FirstOrDefault(track => track.What.Contains("http://kopchik.rpxnow.com"));
			if (startLoginRecord == null) {
				return null;
			}
			var endLoginRecord = log.FirstOrDefault(track => track.When > startLoginRecord.When && track.What == "Page load");
			if (endLoginRecord == null) {
				return null;
			}
			return "Time to login: " + (endLoginRecord.When - startLoginRecord.When).TotalSeconds + "s";
		}

	}

	public class ProjectCreatedRule : IAmImportant {
		public string GetMessage(IEnumerable<TrackerInputModel> log) {
			var validEntries = from entry in log where entry.What.StartsWith("Creating a project") select entry;
			var messages = (from entry in validEntries select GetMessage(entry)).ToArray();
			if (messages.Any()) {
				return messages.Join(Environment.NewLine);
			}
			return null;
		}

		private string GetMessage(TrackerInputModel entry) {
			var serializedModel = entry.What.Substring("Creating a project: ".Length);
			try {
				var model = new JavaScriptSerializer().Deserialize<AddSimpleProjectInputModel>(serializedModel);
				return "Created a project: " + model.OutputType +
				       (model.TemplatePath.IsNotEmpty() ? ", template: " + model.TemplatePath : String.Empty);
			}
			catch (Exception exception) {
				return "Created a project: " + serializedModel + " (" + exception.ToString() + ")";
			}
		}
	}

	public class PreviewDialogOpenedRule : IAmImportant {
		public string GetMessage(IEnumerable<TrackerInputModel> log) {
			var validEntries = from entry in log where entry.What.Contains("publishWeb") select entry;
			return validEntries.Any()? "Opened Web Publish Dialog" : null;
		}
	}
}