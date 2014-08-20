using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using ChpokkWeb.Features.Authentication;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class InterestDetector {
		private readonly UserManager _userManager;
		public InterestDetector(UserManager userManager) {
			_userManager = userManager;
		}


		public InterestStatus ShouldStart(IIdentity userIdentity) {
			if (!userIdentity.IsAuthenticated) return InterestStatus.Newbie;
			var user = _userManager.GetUser(userIdentity.Name);
			if (user == null || user.Status == null) return InterestStatus.Newbie;
			if (user.Status.ToString() == "canceled") return InterestStatus.TrialCanceled;
			if (user.PaidUntil == null || user.PaidUntil > DateTime.Now) return InterestStatus.TrialStarted;
			return InterestStatus.TrialCanceled;
		}


	}

	public enum InterestStatus {
		Newbie,
		TrialCanceled,
		TrialStarted,
		Expired
	}
}