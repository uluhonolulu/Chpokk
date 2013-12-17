using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class BeginEditorExperienceEndpoint {
		private readonly InterestDetector _interestDetector;
		private readonly ISecurityContext _securityContext;
		public BeginEditorExperienceEndpoint(InterestDetector interestDetector, ISecurityContext securityContext) {
			_interestDetector = interestDetector;
			_securityContext = securityContext;
		}

		public BeginEditorExperienceModel DoIt(BeginEditorExperienceDummyInputModel _) {
			var shouldStart = _interestDetector.ShouldStart(_securityContext.CurrentIdentity);
			return new BeginEditorExperienceModel { ShouldStart = shouldStart };
		}

	}

	public class BeginEditorExperienceModel {
		public InterestStatus ShouldStart { get; set; }
	}

	public class BeginEditorExperienceDummyInputModel {}
}