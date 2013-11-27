using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Security;
using Newtonsoft.Json;
using Simple.Data;

namespace ChpokkWeb.Features.Authentication {
	public class UserManager {
		private readonly IAuthenticationContext _authenticationContext;

		public UserManager(IAuthenticationContext authenticationContext) {
			_authenticationContext = authenticationContext;
		}

		public string SigninUser(dynamic profile, string rawData, UserData userData) {
			userData.Profile = profile;
			var userName = GetUsername(profile);
			_authenticationContext.ThisUserHasBeenAuthenticated(userName, true);

			var db = Database.Open();
			var user = db.Users.FindByUserId(userName);
			if (user == null) {
				db.Users.Insert(UserId: userName, FullName: profile.displayName, Email: profile.email, Photo: profile.photo, Data: rawData);
			}
			return userName;
		}

		public dynamic GetUser(string userName) {
			var db = Database.Open();
			return db.Users.FindByUserId(userName);
		}


		private dynamic GetUsername(dynamic profile) {
			var username = (profile.preferredUsername != null) ? profile.preferredUsername.Value : profile.email.Value;
			return username.ToString() + "_" + profile.providerName;
		}
	}
}