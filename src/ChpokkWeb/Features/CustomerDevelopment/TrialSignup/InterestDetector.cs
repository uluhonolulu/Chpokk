using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class InterestDetector {
		private readonly UserHub _userHub;
		public InterestDetector(UserHub userHub) {
			_userHub = userHub;
		}

		public void StartWatching() {
			new Thread(() => {
				Thread.Sleep(10000);
				_userHub.DisplayTrialInvitation();
			});
		}
	}
}