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

		public void DoIt(ClickTrackerInputModel model) {
			var userName = _securityContext.IsAuthenticated() ? _securityContext.CurrentIdentity.Name : null;
			_activityTracker.Record(userName, model.ButtonName, model.Url);
		}

		public void TrackErrors(ErrorModel model) {
			var userName = _securityContext.IsAuthenticated() ? _securityContext.CurrentIdentity.Name : null;
			_activityTracker.RecordException(model);
		}
	}

	public class ClickTrackerInputModel: ITrack {
		public string ButtonName { get; set; }
		public string Url { get; set; }
		public override string ToString() {
			return "Url: {0}, button: {1}".ToFormat(this.Url, this.ButtonName);
		}
	}

	public interface ITrack {}

	public class ErrorModel: ITrack {
		public string Message { get; set; }
		public string Url { get; set; }
		public int LineNumber  { get; set; }
		public string UserAgent { get; set; }
		public string StackTrace { get; set; }
		public override string ToString() {
			return "Url: {0} (line {2}), error: {1}, browser: {3}, \nstack: {4}".ToFormat(this.Url, this.Message, this.LineNumber, this.UserAgent, this.StackTrace);
		}
	}
}