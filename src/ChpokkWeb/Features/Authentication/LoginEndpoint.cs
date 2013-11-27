using System;
using System.Net;
using System.Net.Mail;
using ChpokkWeb.Features.MainScreen;
using FubuMVC.Core.Continuations;
using Newtonsoft.Json;

namespace ChpokkWeb.Features.Authentication {
	public class LoginEndpoint {
		private readonly SmtpClient _mailer;
		private readonly UserManager _userManager;
		private UserData _userData;

		public LoginEndpoint(SmtpClient mailer, UserManager userManager) {
			_mailer = mailer;
			_userManager = userManager;
		}

		public FubuContinuation Login(LoginInputModel model) {

			var url = string.Format("https://rpxnow.com/api/v2/auth_info?apiKey={0}&token={1}", model.ApiKey, model.token);
			var rawResponse =
				new WebClient().DownloadString(url);
			var response = JsonConvert.DeserializeObject<dynamic>(rawResponse);
			if (response.stat.ToString() == "ok") {
				try {
					var username = _userManager.SigninUser(response.profile, rawResponse, _userData);
					if(_mailer.Host != null) _mailer.Send("features@chpokk.apphb.com", "uluhonolulu@gmail.com", "New user: " + username, rawResponse);
					return FubuContinuation.RedirectTo<MainDummyModel>();
				}
				catch (Exception exception) {
					throw new ApplicationException("Authentification error: " + rawResponse, exception);
				}
			}
			return FubuContinuation.EndWithStatusCode(HttpStatusCode.Unauthorized);
		}
	}

	public class LoginInputModel {
		public string token { get; set; }
		public string ApiKey = "3a86d86155ae6ed54397f3e8a9aa09294151bd7b";
	}
}