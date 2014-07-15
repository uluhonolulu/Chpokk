using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.CustomerDevelopment {
	public class TrackerEndpoint {
		private readonly ActivityTracker _activityTracker;
		private readonly ISecurityContext _securityContext;
		public TrackerEndpoint(ActivityTracker activityTracker, ISecurityContext securityContext) {
			_activityTracker = activityTracker;
			_securityContext = securityContext;
		}

		public void DoIt(TrackerInputModel model) {
			var userName = _securityContext.IsAuthenticated() ? _securityContext.CurrentIdentity.Name : null;
			_activityTracker.Record(userName, model.What, model.Url, model.Browser);
		}

		public void TrackErrors(ErrorModel model) {
			var userName = _securityContext.IsAuthenticated() ? _securityContext.CurrentIdentity.Name : null;
			_activityTracker.RecordException(model);
		}
	}

	public class TrackerInputModel: ITrack {
		public TrackerInputModel() {
			When = DateTime.Now;
		}
		public string What { get; set; }
		public string Url { get; set; }
		public DateTime When { get; private set; }
		public string Browser { get; set; }
		public override string ToString() {
			return "{2}: Url: {0}, what: {1}".ToFormat(this.Url, this.What, this.When.ToString("HH:mm:ss"));
		}
	}

	public interface ITrack {
		DateTime When { get; }
	}

	public class ErrorModel: ITrack {
		public ErrorModel() {
			When = DateTime.Now;
		}
		public string Message { get; set; }
		public string Url { get; set; }
		public int LineNumber  { get; set; }
		public string UserAgent { get; set; }
		public string StackTrace { get; set; }
		public DateTime When { get; private set; }
		public override string ToString() {
			return "Url: {0} (line {2}), error: {1}, browser: {3}, \nstack: {4}\n".ToFormat(this.Url, this.Message, this.LineNumber, this.UserAgent, this.StackTrace);
		}
	}
}