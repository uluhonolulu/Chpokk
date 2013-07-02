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
				//microsoft
				/*+		response.profile	{
				  "providerName": "Microsoft Account",
				  "identifier": "http://cid-e5c68243cd4aed39.spaces.live.com/",
				  "name": {},
				  "email": "gamma@mnogomango.spb.ru",
				  "providerSpecifier": "microsoftaccount"
				}	dynamic {Newtonsoft.Json.Linq.JObject}
				*/
				var username = (response.profile.preferredUsername != null) ? response.profile.preferredUsername.Value : response.profile.email.Value;
				_authenticationContext.ThisUserHasBeenAuthenticated(username, true);
				if(_mailer.Host != null) _mailer.Send("features@chpokk.apphb.com", "uluhonolulu@gmail.com", "New user: " + username, rawResponse);
				return FubuContinuation.RedirectTo<MainDummyModel>();
			}
			return FubuContinuation.EndWithStatusCode(HttpStatusCode.Unauthorized);
		}

		private string GetUsernameFromEmail(string email) {
			return null;
		}
	}

	public class LoginInputModel {
		public string token { get; set; }
		public string ApiKey = "3a86d86155ae6ed54397f3e8a9aa09294151bd7b";
	}
}