using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Authentication;
using FubuMVC.Core.Ajax;
using Gotcha;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class StartTrialEndpoint {
		private readonly UserManager _userManager;
		private Chimper _chimper;
		public StartTrialEndpoint(UserManager userManager, Chimper chimper) {
			_userManager = userManager;
			_chimper = chimper;
		}

		public AjaxContinuation StartTrial(StartTrialDummyInputModel _) {
			UpdateUser();
			var user = _userManager.GetCurrentUser();
			_chimper.SubscribeUser(user.Email, user.FullName);
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