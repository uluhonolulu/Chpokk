using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Infrastructure {
	public class AllowOnlyAuthenticatedAuthorizationPolicy: IAuthorizationPolicy {
		private readonly ISecurityContext _securityContext;
		public AllowOnlyAuthenticatedAuthorizationPolicy(ISecurityContext securityContext) {
			_securityContext = securityContext;
		}

		public AuthorizationRight RightsFor(IFubuRequest request) {
			if (_securityContext.IsAuthenticated()) {
				return AuthorizationRight.Allow;
			}
			else {
				return AuthorizationRight.Deny;
			}
		}
	}
}