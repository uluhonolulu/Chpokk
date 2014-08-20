using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
		private readonly IUrlRegistry _urlRegistry;
		private readonly SmtpClient _mailer;
		public StartTrialEndpoint(UserManagerInContext userManager, Chimper chimper, IUrlRegistry urlRegistry, SmtpClient mailer) {
			_userManager = userManager;
			_chimper = chimper;
			_urlRegistry = urlRegistry;
			_mailer = mailer;
		}

		public AjaxContinuation StartTrial(WannaPayDummyInputModel _) {
			var user = _userManager.GetCurrentUser();
			//we might have been disconnected since then
			if (user == null) {
				return new AjaxContinuation{NavigatePage = _urlRegistry.UrlFor<MainDummyModel>()};
			}
			if (_mailer.Host != null) _mailer.Send("signups@chpokk.apphb.com", "uluhonolulu@gmail.com", "New signup: " + user.UserId, "GODDAMIT!!!!!!");
			return new AjaxContinuation { NavigatePage = "https://sites.fastspring.com/geeksoft/instant/chpokkstarter?referer=" + user.UserId };
		}

		private void UpdateUser() {
			var user = _userManager.GetCurrentUser();
			user.Status = (string) UserStatus.Trial;
			user.PaidUntil = DateTime.Today.Add(TimeSpan.FromDays(10));
			_userManager.UpdateUser(user);
		}
	}

	public class WannaPayDummyInputModel {}
}