using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class UserHub: Hub {
		private readonly InterestDetector _interestDetector;
		public UserHub(InterestDetector interestDetector) {
			_interestDetector = interestDetector;
		}

		public void OnStartPlayingWith() {
			if (_interestDetector.ShouldStart()) {
				new Thread(() => {
					Thread.Sleep(10000);
					Clients.Caller.displayTrialInvitation();
				});				
			}
		}

		//public void DisplayTrialInvitation() {
		//	Clients.All.displayTrialInvitation();
		//}
	}
}