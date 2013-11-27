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


		public bool ShouldStart() {
			bool shouldStart;
			if (!_securityContext.IsAuthenticated()) shouldStart = false;
			else {
				var userName = _securityContext.CurrentIdentity.Name;
				var user = _userManager.GetUser(userName);
				shouldStart = user.Status == null;
			}
			return shouldStart;
		}

		//public void StartWatching() {
		//	new Thread(() => {
		//		Thread.Sleep(10000);
		//		_userHub.DisplayTrialInvitation();
		//	});
		//}
	}
}