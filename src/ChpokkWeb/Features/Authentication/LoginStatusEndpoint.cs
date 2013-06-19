using FubuMVC.Core.Continuations;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.Authentication {
	public class LoginStatusEndpoint {
		private readonly ISecurityContext _securityContext;
		public LoginStatusEndpoint(ISecurityContext securityContext) {
			_securityContext = securityContext;
		}

		public string LoginStatus() {
			//return "<a class=\"btn btn-primary\" href=\"/authentication/login\">Sign-In</a>";
			if (_securityContext.IsAuthenticated()) {
				return "<div style='padding-top:12px;'>" + _securityContext.CurrentIdentity.Name + "</div>";
			}
			return "<a class=\"janrainEngage btn btn-primary\" style='margin-top:50px;'>Sign-In</a>";
		}


	}
}