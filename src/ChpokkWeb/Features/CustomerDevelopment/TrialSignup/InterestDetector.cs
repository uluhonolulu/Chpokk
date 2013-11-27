using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using ChpokkWeb.Features.Authentication;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class InterestDetector {
		private readonly ISecurityContext _securityContext;
		private readonly UserManager _userManager;
		public InterestDetector(ISecurityContext securityContext, UserManager userManager) {
			_securityContext = securityContext;
			_userManager = userManager;
		}


		public InterestStatus ShouldStart() {
			if (!_securityContext.IsAuthenticated()) return InterestStatus.Newbie;
			var userName = _securityContext.CurrentIdentity.Name;
			var user = _userManager.GetUser(userName);
			if (user.Status == null) return InterestStatus.Newbie;
			if (user.Status.ToString() == "canceled") return InterestStatus.TrialCanceled;
			return InterestStatus.TrialStarted;
		}


	}

	public enum InterestStatus {
		Newbie,
		TrialCanceled,
		TrialStarted
	}
}