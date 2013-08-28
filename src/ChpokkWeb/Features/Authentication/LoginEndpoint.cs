using System;
using System.Net;
using System.Net.Mail;
using ChpokkWeb.Features.MainScreen;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Security;
using Newtonsoft.Json;

namespace ChpokkWeb.Features.Authentication {
	public class LoginEndpoint {
		private readonly IAuthenticationContext _authenticationContext;
		private readonly SmtpClient _mailer;
		private readonly UserData _userData;

		public LoginEndpoint(IAuthenticationContext authenticationContext, SmtpClient mailer, UserData userData) {
			_authenticationContext = authenticationContext;
			_mailer = mailer;
			_userData = userData;
		}

		public FubuContinuation Login(LoginInputModel model) {

			var url = string.Format("https://rpxnow.com/api/v2/auth_info?apiKey={0}&token={1}", model.ApiKey, model.token);
			var rawResponse =
				new WebClient().DownloadString(url);
			var response = JsonConvert.DeserializeObject<dynamic>(rawResponse);
			if (response.stat.ToString() == "ok") {
				try {
					dynamic profile = response.profile;
					_userData.Profile = profile;
					var username = GetUsername(profile);
					_authenticationContext.ThisUserHasBeenAuthenticated(username, true);
					if(_mailer.Host != null) _mailer.Send("features@chpokk.apphb.com", "uluhonolulu@gmail.com", "New user: " + username, rawResponse);
					return FubuContinuation.RedirectTo<MainDummyModel>();
				}
				catch (Exception exception) {
					if(_mailer.Host != null) _mailer.Send("features@chpokk.apphb.com", "uluhonolulu@gmail.com", "Authentification error", rawResponse);
					throw;
				}
			}
			return FubuContinuation.EndWithStatusCode(HttpStatusCode.Unauthorized);
		}

		private dynamic GetUsername(dynamic profile) {
			var username = (profile.preferredUsername != null) ? profile.preferredUsername.Value : profile.email.Value;
			return username.ToString() + "_" + profile.providerName;
		}
	}

	public class LoginInputModel {
		public string token { get; set; }
		public string ApiKey = "3a86d86155ae6ed54397f3e8a9aa09294151bd7b";
	}
}