using FubuMVC.Core.Continuations;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.Authentication {
	public class LoginStatusEndpoint {
		private readonly ISecurityContext _securityContext;
		private readonly UserData _userData;
		private UserManager _userManager;
		public LoginStatusEndpoint(ISecurityContext securityContext, UserData userData, UserManager userManager) {
			_securityContext = securityContext;
			_userData = userData;
			_userManager = userManager;
		}

		public FubuContinuation LoginStatus() {
			//return "<a class=\"btn btn-primary\" href=\"/authentication/login\">Sign-In</a>";
			if (_securityContext.IsAuthenticated()) {
				_userManager.EnsureUserData(_securityContext.CurrentIdentity.Name);
				return FubuContinuation.TransferTo(new LoginStatusAuthenticatedModel(_userData));
				//return "<div style='padding-top:12px;'>" + _securityContext.CurrentIdentity.Name + "</div>";
			}
			return FubuContinuation.TransferTo(new LoginStatusAnonModel());
			//return "<a class=\"janrainEngage btn btn-primary\">Sign-In</a>";
		}


	}
}