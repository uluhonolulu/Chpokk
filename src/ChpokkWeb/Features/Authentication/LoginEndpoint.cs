﻿using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using ChpokkWeb.Features.CustomerDevelopment;
using ChpokkWeb.Features.MainScreen;
using FubuMVC.Core.Continuations;
using Newtonsoft.Json;

namespace ChpokkWeb.Features.Authentication {
	public class LoginEndpoint {
		private readonly SmtpClient _mailer;
		private readonly UserManager _userManager;
		private UserData _userData;
		private ActivityTracker _tracker;

		public LoginEndpoint(SmtpClient mailer, UserManager userManager, UserData userData, ActivityTracker tracker) {
			_mailer = mailer;
			_userManager = userManager;
			_userData = userData;
			_tracker = tracker;
		}

		public FubuContinuation Login(LoginInputModel model) {
			_tracker.Record(null, "Starting login, calling Janrain", null, null);
			var url = string.Format("https://rpxnow.com/api/v2/auth_info?apiKey={0}&token={1}", model.ApiKey, model.token);
			var rawResponse =
				new WebClient(){Encoding = Encoding.UTF8}.DownloadString(url);
			_tracker.Record(null, "Called Janrain", null, null);
			var response = JsonConvert.DeserializeObject<dynamic>(rawResponse);
			if (response.stat.ToString() == "ok") {
				try {
					var username = _userManager.SigninUser(response.profile, rawResponse, _userData);
					_tracker.Record(null, "Signed in", null, null);
					if (_mailer.Host != null) _mailer.Send("features@chpokk.apphb.com", "uluhonolulu@gmail.com", "New user: " + username, rawResponse);
					_tracker.Record(null, "Sent notification", null, null);
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