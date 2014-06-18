using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.MainScreen;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class StartTrialEndpoint {
		private readonly UserManagerInContext _userManager;
		private readonly Chimper _chimper;
		private IUrlRegistry _urlRegistry;
		public StartTrialEndpoint(UserManagerInContext userManager, Chimper chimper, IUrlRegistry urlRegistry) {
			_userManager = userManager;
			_chimper = chimper;
			_urlRegistry = urlRegistry;
		}

		public AjaxContinuation StartTrial(StartTrialDummyInputModel _) {
			var user = _userManager.GetCurrentUser();
			//we might have been disconnected since then
			if (user == null) {
				return new AjaxContinuation{NavigatePage = _urlRegistry.UrlFor<MainDummyModel>()};
			}
			UpdateUser();
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