using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class UserHub: Hub {
		private readonly InterestDetector _interestDetector;
		private static readonly TimeSpan evaluationTime = TimeSpan.FromMinutes(1);

		public UserHub(InterestDetector interestDetector) {
			_interestDetector = interestDetector;
		}

		public string OnStartPlayingWith() {
			var userStatus = _interestDetector.ShouldStart(Context.User.Identity);
			return userStatus.ToString();
		}

		//public void DisplayTrialInvitation() {
		//	Clients.All.displayTrialInvitation();
		//}
	}
}