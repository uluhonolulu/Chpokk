using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.Authentication {
	public class UserData {
		public dynamic Profile { get; set; }
		public string DisplayName { get {
			return Profile != null ? Profile.displayName : null;
		}
		}
		public string Photo { get {
			return Profile != null ? Profile.photo : null;
		}
		}
	}
}