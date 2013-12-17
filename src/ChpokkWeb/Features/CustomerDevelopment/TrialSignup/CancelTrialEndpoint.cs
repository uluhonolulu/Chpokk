using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Authentication;
using FubuMVC.Core.Ajax;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class CancelTrialEndpoint {
		private readonly UserManagerInContext _userManager;
		public CancelTrialEndpoint(UserManagerInContext userManager) {
			_userManager = userManager;
		}

		public AjaxContinuation CancelTrial(CancelTrialDummyInputModel _) {
			UpdateUser();
			return new AjaxContinuation().NavigateTo("/");
		}

		private void UpdateUser() {
			var user = _userManager.GetCurrentUser();
			if (user != null) {
				user.Status = (string)UserStatus.Canceled;
				_userManager.UpdateUser(user);
			}
		}
	}

	public class CancelTrialDummyInputModel {}
}