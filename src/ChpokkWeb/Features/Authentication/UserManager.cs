using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using FubuMVC.Core.Security;
using Newtonsoft.Json;
using Simple.Data;

namespace ChpokkWeb.Features.Authentication {
	public class UserManager {
		protected readonly IAuthenticationContext _authenticationContext;

		public UserManager(IAuthenticationContext authenticationContext) {
			_authenticationContext = authenticationContext;
		}

		public string SigninUser(dynamic profile, string rawData, UserData userData) {
			userData.Profile = profile;
			var userName = GetUsername(profile);
			_authenticationContext.ThisUserHasBeenAuthenticated(userName, true);

			new Thread(() => SaveUser(profile, rawData)).Start();
			//Task.Run(() => SaveUser(profile, rawData));
			return userName;
		}

		private void SaveUser(dynamic profile, string rawData) {
			var userName = GetUsername(profile);
			var db = Database.Open();
			var user = db.Users.FindByUserId(userName);
			if (user == null) {
				db.Users.Insert(UserId: userName, FullName: profile.displayName, Email: profile.email, Photo: profile.photo,
				                Data: rawData);
			}
		}

		public void SignoutUser() {
			_authenticationContext.SignOut();
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
			var username = profile.identifier;
			if (profile.displayName != null) username = profile.displayName.Value;
			if (profile.email != null) username = profile.email.Value;
			if (profile.preferredUsername != null) username = profile.preferredUsername.Value;
			foreach (var invalidPathChar in Path.GetInvalidPathChars()) {
				username = ((string) username).Replace(invalidPathChar, '_'); 
			}
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

		public void ForceUnregisteredUserToSignout() {
			if (!_securityContext.IsAuthenticated()) {
				return;
			}
			if (GetCurrentUser() == null) {
				_authenticationContext.SignOut();
			}
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