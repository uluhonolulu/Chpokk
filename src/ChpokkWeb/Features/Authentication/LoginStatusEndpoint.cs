using FubuMVC.Core.Continuations;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.Authentication {
	public class LoginStatusEndpoint {
		private ISecurityContext _securityContext;
		public LoginStatusEndpoint(ISecurityContext securityContext) {
			_securityContext = securityContext;
		}

		public FubuContinuation LoginStatus() {
			if (_securityContext.IsAuthenticated()) {
				return
					FubuContinuation.TransferTo(new AuthenticatedStatusModel {UserName = _securityContext.CurrentIdentity.Name});
			}

		}
	}
}