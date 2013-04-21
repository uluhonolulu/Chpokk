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
				return _securityContext.CurrentIdentity.Name;
			}
			return "<a class=\"janrainEngage btn btn-primary\">Sign-In</a>";
		}


	}
}