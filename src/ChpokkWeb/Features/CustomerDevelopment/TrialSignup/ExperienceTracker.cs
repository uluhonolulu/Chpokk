using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class ExperienceTracker {
		private bool _tracking = false;
		private static readonly TimeSpan delay = TimeSpan.FromMinutes(20);

		public void StartTracking(Action onFinished) {
			if (!_tracking) {
				_tracking = true;
				Task.Run(() =>
				{
					Thread.Sleep(delay);
					_tracking = false;
					onFinished(); 
				});
			}
		}

	}
}