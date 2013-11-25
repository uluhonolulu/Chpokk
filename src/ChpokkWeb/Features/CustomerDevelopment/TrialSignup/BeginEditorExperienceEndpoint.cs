using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Authentication;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class BeginEditorExperienceEndpoint {
		private readonly ISecurityContext _securityContext;
		private readonly UserManager _userManager;
		public BeginEditorExperienceEndpoint(ISecurityContext securityContext, UserManager userManager) {
			_securityContext = securityContext;
			_userManager = userManager;
		}

		public BeginEditorExperienceModel DoIt() {
			bool shouldStart;
			if (!_securityContext.IsAuthenticated()) {
				shouldStart = false;
			}
			else {
				var userName = _securityContext.CurrentIdentity.Name;
				var user = _userManager.GetUser(userName);
				shouldStart = user.Status == null;
			}
			return new BeginEditorExperienceModel { ShouldStart = shouldStart };
		}
	}

	public class BeginEditorExperienceModel {
		public bool ShouldStart { get; set; }
	}
}