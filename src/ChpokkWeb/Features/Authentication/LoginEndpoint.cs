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

		public LoginEndpoint(IAuthenticationContext authenticationContext, SmtpClient mailer) {
			_authenticationContext = authenticationContext;
			_mailer = mailer;
		}

		public FubuContinuation Login(LoginInputModel model) {

			var url = string.Format("https://rpxnow.com/api/v2/auth_info?apiKey={0}&token={1}", model.ApiKey, model.token);
			var rawResponse =
				new WebClient().DownloadString(url);
			var response = JsonConvert.DeserializeObject<dynamic>(rawResponse);
			if (response.stat.ToString() == "ok") {
				var username = response.profile.preferredUsername.ToString();
				_authenticationContext.ThisUserHasBeenAuthenticated(username, true);
				if(_mailer.Host != null) _mailer.Send("features@chpokk.apphb.com", "uluhonolulu@gmail.com", "New user: " + username, rawResponse);
				return FubuContinuation.RedirectTo<MainDummyModel>();
			}
			return FubuContinuation.EndWithStatusCode(HttpStatusCode.Unauthorized);
		}

	}

	public class LoginInputModel {
		public string token { get; set; }
		public string ApiKey = "4730b35eb9361c5cccd8a9f14353e8531727f457";
	}
}