using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Ajax;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class StartTrialEndpoint {
		private readonly UserManagerInContext _userManager;
		private readonly Chimper _chimper;
		public StartTrialEndpoint(UserManagerInContext userManager, Chimper chimper) {
			_userManager = userManager;
			_chimper = chimper;
		}

		public AjaxContinuation StartTrial(StartTrialDummyInputModel _) {
			UpdateUser();
			var user = _userManager.GetCurrentUser();
			if (user.Email != null) {
				dynamic result = _chimper.SubscribeUser(user.Email, user.FullName);
			}
			return AjaxContinuation.Successful();
		}

		private void UpdateUser() {
			var user = _userManager.GetCurrentUser();
			user.Status = (string) UserStatus.Trial;
			user.PaidUntil = DateTime.Today.Add(TimeSpan.FromDays(10));
			_userManager.UpdateUser(user);
		}
	}

	public class StartTrialDummyInputModel {}
}