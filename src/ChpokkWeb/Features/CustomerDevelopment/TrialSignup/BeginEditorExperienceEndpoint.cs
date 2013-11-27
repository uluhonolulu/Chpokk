using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class BeginEditorExperienceEndpoint {
		private readonly InterestDetector _interestDetector;
		public BeginEditorExperienceEndpoint(InterestDetector interestDetector) {
			_interestDetector = interestDetector;
		}

		public BeginEditorExperienceModel DoIt(BeginEditorExperienceDummyInputModel _) {
			var shouldStart = _interestDetector.ShouldStart();
			return new BeginEditorExperienceModel { ShouldStart = shouldStart };
		}

	}

	public class BeginEditorExperienceModel {
		public InterestStatus ShouldStart { get; set; }
	}

	public class BeginEditorExperienceDummyInputModel {}
}