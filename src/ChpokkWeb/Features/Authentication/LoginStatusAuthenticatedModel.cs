using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.Authentication {
	public class LoginStatusAuthenticatedModel {
		public LoginStatusAuthenticatedModel(UserData userData) {
			UserData = userData;
		}

		public UserData UserData { get; private set; }
		public string Username { get; set; }
		public string DisplayName {
			get {
				if (UserData != null && UserData.DisplayName != null) {
					return UserData.DisplayName;
				}
				return Username;
			}
		}
	}
}