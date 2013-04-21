using System.Net;
using ChpokkWeb.Features.MainScreen;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Security;
using Newtonsoft.Json;

namespace ChpokkWeb.Features.Authentication {
	public class LoginEndpoint {
		private readonly IAuthenticationContext _authenticationContext;
		private ISecurityContext _securityContext;
		public LoginEndpoint(IAuthenticationContext authenticationContext, ISecurityContext securityContext) {
			_authenticationContext = authenticationContext;
			_securityContext = securityContext;
		}

		public FubuContinuation Login(LoginInputModel model) {
			//_authenticationContext.ThisUserHasBeenAuthenticated("ulu", true);
			//return FubuContinuation.RedirectTo<ShowMeModel>();

			var url = string.Format("https://rpxnow.com/api/v2/auth_info?apiKey={0}&token={1}", model.ApiKey, model.token);
			var rawResponse =
				new WebClient().DownloadString(url);
			var response = JsonConvert.DeserializeObject<dynamic>(rawResponse);
			if (response.stat.ToString() == "ok") {
				var username = response.profile.preferredUsername.ToString();
				_authenticationContext.ThisUserHasBeenAuthenticated(username, true);
				return FubuContinuation.RedirectTo<MainDummyModel>();
			}
			return FubuContinuation.EndWithStatusCode(HttpStatusCode.Unauthorized);
			//return AjaxContinuation.ForMessage(response.stat.ToString());
		}

		public string ShowMe(ShowMeModel model) {
			return _securityContext.IsAuthenticated().ToString();
		}

		public class ShowMeModel {}
	}

	public class LoginInputModel {
		public string token { get; set; }
		public string ApiKey = "4730b35eb9361c5cccd8a9f14353e8531727f457";
	}
}