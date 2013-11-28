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
			switch (_interestDetector.ShouldStart()) {
					case InterestStatus.Newbie:
						new Thread(() => {
							Thread.Sleep(TimeSpan.FromMinutes(1));
							Clients.Caller.displayTrialInvitation();
						}).Start(); break;
					case InterestStatus.TrialCanceled:
						Clients.Caller.displayTrialInvitation(); break;
			}
		}

		//public void DisplayTrialInvitation() {
		//	Clients.All.displayTrialInvitation();
		//}
	}
}