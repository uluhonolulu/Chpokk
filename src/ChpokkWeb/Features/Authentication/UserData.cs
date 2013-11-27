using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Security;
using Newtonsoft.Json;
using Simple.Data;

namespace ChpokkWeb.Features.Authentication {
	public class UserData {
		private readonly UserManager _userManager;
		private readonly ISecurityContext _securityContext;

		private dynamic _profile;
		public UserData(UserManager userManager, ISecurityContext securityContext) {
			_userManager = userManager;
			_securityContext = securityContext;
		}

		public dynamic Profile {
			get {
				if (_profile == null && _securityContext.IsAuthenticated()) {
					var userName = _securityContext.CurrentIdentity.Name;
					var user = _userManager.GetUser(userName);
					Profile = JsonConvert.DeserializeObject<dynamic>(user.Data).profile;
				}
				return _profile;
			}
			set { _profile = value; }
		}

		public string DisplayName {
			get {
				return Profile != null ? Profile.displayName : null;
			}
		}
		public string Photo {
			get {
				return Profile != null ? Profile.photo : null;
			}
		}
	}
}