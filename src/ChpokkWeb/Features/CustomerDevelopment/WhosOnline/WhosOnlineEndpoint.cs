using System;
using System.Collections.Generic;

namespace ChpokkWeb.Features.CustomerDevelopment.WhosOnline {
	public class WhosOnlineEndpoint {
		private readonly WhosOnlineTracker _tracker;
		public WhosOnlineEndpoint(WhosOnlineTracker tracker) {
			_tracker = tracker;
		}

		public string DoIt() {
			return _tracker.Who.Join(Environment.NewLine);
		}
	}
}