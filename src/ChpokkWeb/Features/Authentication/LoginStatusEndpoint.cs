using FubuMVC.Core.Continuations;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.Authentication {
	public class LoginStatusEndpoint {
		private readonly ISecurityContext _securityContext;
		private readonly UserData _userData;

		public LoginStatusEndpoint(ISecurityContext securityContext, UserData userData, UserManager userManager) {
			_securityContext = securityContext;
			_userData = userData;
		}

		public FubuContinuation LoginStatus() {
			if (_securityContext.IsAuthenticated()) {
				return FubuContinuation.TransferTo(new LoginStatusAuthenticatedModel(_userData));
			}
			return FubuContinuation.TransferTo(new LoginStatusAnonModel());
		}


	}
}