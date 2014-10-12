using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.MainScreen;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class StartTrialEndpoint {
		private readonly UserManagerInContext _userManager;
		private readonly Chimper _chimper;
		private readonly IUrlRegistry _urlRegistry;
		private readonly SmtpClient _mailer;
		private readonly IAppRootProvider _appRootProvider;
		private readonly IFileSystem _fileSystem;
		public StartTrialEndpoint(UserManagerInContext userManager, Chimper chimper, IUrlRegistry urlRegistry, SmtpClient mailer, IAppRootProvider appRootProvider, IFileSystem fileSystem) {
			_userManager = userManager;
			_chimper = chimper;
			_urlRegistry = urlRegistry;
			_mailer = mailer;
			_appRootProvider = appRootProvider;
			_fileSystem = fileSystem;
		}

		public AjaxContinuation StartTrial(WannaPayDummyInputModel _) {
			var user = _userManager.GetCurrentUser();
			//we might have been disconnected since then
			if (user == null) {
				return new AjaxContinuation{NavigatePage = _urlRegistry.UrlFor<MainDummyModel>()};
			}
			SendMessageAfterDelay(user.UserId);
			if (_mailer.Host != null) _mailer.Send("signups@chpokk.apphb.com", "uluhonolulu@gmail.com", "New signup: " + user.UserId, "GODDAMIT!!!!!!");
			return new AjaxContinuation { NavigatePage = "https://sites.fastspring.com/geeksoft/instant/chpokkstarter?referer=" + user.UserId };
		}

		private async void SendMessageAfterDelay(string userName) {
			Task.Run(async () =>
			{
				await Task.Delay(TimeSpan.FromMinutes(10)); //wait 10min before the guy decides not to buy
				var user = _userManager.GetUser(userName);
				if (user.Status == "canceled") {
					var messageTemplatePath =
						_appRootProvider.AppRoot.AppendPath(@"Features\CustomerDevelopment\TrialSignup\WhyDidntYouBuy.txt");
					var messageToUser = _fileSystem.ReadStringFromFile(messageTemplatePath);

					if (_mailer.Host != null)
						_mailer.Send("uluhonolulu@gmail.com", user.Email, "A free month of Chpokk. Interested?", messageToUser);
					
				}
			});
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