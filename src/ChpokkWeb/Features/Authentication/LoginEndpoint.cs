using System;
using System.Net;
using System.Net.Mail;
using System.Security.Authentication;
using System.Text;
using ChpokkWeb.Features.CustomerDevelopment;
using ChpokkWeb.Features.MainScreen;
using ChpokkWeb.Infrastructure.Logging;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Continuations;
using Newtonsoft.Json;

namespace ChpokkWeb.Features.Authentication {
	public class LoginEndpoint {
		private readonly SmtpClient _mailer;
		private readonly UserManager _userManager;
		private readonly UserData _userData;
		private readonly ActivityTracker _tracker;

		public LoginEndpoint(SmtpClient mailer, UserManager userManager, UserData userData, ActivityTracker tracker) {
			_mailer = mailer;
			_userManager = userManager;
			_userData = userData;
			_tracker = tracker;
		}

		public AjaxContinuation Login(LoginInputModel model) {
			var logger = SimpleLogger.CreateLogger(model.ConnectionId);
			_tracker.Record("Starting login, calling Janrain");
			logger.Log("Verifying your credentials..");
			var url = string.Format("https://rpxnow.com/api/v2/auth_info?apiKey={0}&token={1}", model.ApiKey, model.token);
			var rawResponse =
				new WebClient(){Encoding = Encoding.UTF8}.DownloadString(url);
			_tracker.Record("Called Janrain");
			var response = JsonConvert.DeserializeObject<dynamic>(rawResponse);
			if (response.stat.ToString() == "ok") {
				try {
					logger.Log("Registering you in the system..");
					_tracker.Record("Signing in");
					var username = _userManager.SigninUser(response.profile, rawResponse, _userData);
					_tracker.Record("Signed in");
					logger.Log("Reloading the page..");
					//if (_mailer.Host != null) _mailer.Send("features@chpokk.apphb.com", "uluhonolulu@gmail.com", "New user: " + username, rawResponse);
					//_tracker.Record("Sent notification");
					return new AjaxContinuation() { ShouldRefresh = true };
				}
				catch (Exception exception) {
					throw new AuthenticationException("Authentification error: " + rawResponse, exception);
				}
			}
			throw new AuthenticationException("Authentification error: " + response.err.msg); 
		}
	}

	public class LoginInputModel {
		public string token { get; set; }

		public string ConnectionId { get; set; }

		public string ApiKey = "3a86d86155ae6ed54397f3e8a9aa09294151bd7b";
	}
}