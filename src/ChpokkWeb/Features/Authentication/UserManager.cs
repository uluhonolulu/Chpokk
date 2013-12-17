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

		public void UpdateUser(dynamic user) {
			var db = Database.Open();
			db.Users.Update(user);
		}

		private dynamic GetUsername(dynamic profile) {
			var username = (profile.preferredUsername != null) ? profile.preferredUsername.Value : profile.email.Value;
			return username.ToString() + "_" + profile.providerName;
		}
	}

	public class UserManagerInContext: UserManager {
		private readonly ISecurityContext _securityContext;

		public UserManagerInContext(IAuthenticationContext authenticationContext, ISecurityContext securityContext) : base(authenticationContext) {
			_securityContext = securityContext;
		}

		public dynamic GetCurrentUser() {
			if (!_securityContext.IsAuthenticated()) {
				return null;
			}
			var userName = _securityContext.CurrentIdentity.Name;
			return GetUser(userName);
		}
	}

	public class UserStatus {
		public static readonly UserStatus Trial = new UserStatus{Status = "trial"};
		public static readonly UserStatus Base = new UserStatus{Status = "base"};
		public static readonly UserStatus Canceled = new UserStatus{Status = "canceled"};


		private string Status { get; set; }
		public static implicit operator string(UserStatus status) {
			return status.Status;
		}
	}
}