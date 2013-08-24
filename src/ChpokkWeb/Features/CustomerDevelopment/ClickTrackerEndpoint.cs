using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.CustomerDevelopment {
	public class ClickTrackerEndpoint {
		private readonly ActivityTracker _activityTracker;
		public ClickTrackerEndpoint(ActivityTracker activityTracker) {
			_activityTracker = activityTracker;
		}

		public void DoIt(ClickTrackerInputModel model) {
			_activityTracker.Record(model);
		}
	}

	public class ClickTrackerInputModel {
		public string ButtonName { get; set; }
		public string Url { get; set; }
	}
}