using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using ChpokkWeb.Features.MainScreen;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Security;
using Newtonsoft.Json;

namespace ChpokkWeb.Features.Authorization {
	public class LoginEndpoint {
		private readonly IAuthenticationContext _authenticationContext;
		public LoginEndpoint(IAuthenticationContext authenticationContext) {
			_authenticationContext = authenticationContext;
		}

		public FubuContinuation Login(LoginInputModel model) {
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
	}

	public class LoginInputModel {
		public string token { get; set; }
		public string ApiKey = "4730b35eb9361c5cccd8a9f14353e8531727f457";
	}
}