using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.CustomerDevelopment {
	public class ClickTrackerEndpoint {
		private readonly ActivityTracker _activityTracker;
		private readonly ISecurityContext _securityContext;
		public ClickTrackerEndpoint(ActivityTracker activityTracker, ISecurityContext securityContext) {
			_activityTracker = activityTracker;
			_securityContext = securityContext;
		}

		public void DoIt(ClickTrackerInputModel model) {
			var userName = _securityContext.IsAuthenticated() ? _securityContext.CurrentIdentity.Name : null;
			_activityTracker.Record(userName, model.ButtonName, model.Url);
		}
	}

	public class ClickTrackerInputModel {
		public string ButtonName { get; set; }
		public string Url { get; set; }
	}
}