using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Security;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class BeginEditorExperienceEndpoint {
		private readonly InterestDetector _interestDetector;
		private readonly ISecurityContext _securityContext;
		private ExperienceTracker _experienceTracker;
		public BeginEditorExperienceEndpoint(InterestDetector interestDetector, ISecurityContext securityContext, ExperienceTracker experienceTracker) {
			_interestDetector = interestDetector;
			_securityContext = securityContext;
			_experienceTracker = experienceTracker;
		}

		public BeginEditorExperienceModel DoIt(BeginEditorExperienceDummyInputModel _) {
			var shouldStart = _interestDetector.ShouldStart(_securityContext.CurrentIdentity);
			//newbies should start counting 20min
			//trials should ignore
			//cancels should display immediately
			//need to start the AJAX req from the client, cause need ConnectionID
			return new BeginEditorExperienceModel { ShouldStart = shouldStart };
		}

		public AjaxContinuation StartTiming(StartTimingInputModel model) {
			_experienceTracker.StartTracking(() => DisplayMessage(model.ConnectionId));
			return AjaxContinuation.Successful();
		}

		private void DisplayMessage(string connectionId) {
			var hubContext = GlobalHost.ConnectionManager.GetHubContext<UserHub>();
			var client = hubContext.Clients.Client(connectionId);
			client.displayTrialInvitation();
		}
	}

	public class StartTimingInputModel {
		public string ConnectionId { get; set; }
	}

	public class BeginEditorExperienceModel {
		public InterestStatus ShouldStart { get; set; }
	}

	public class BeginEditorExperienceDummyInputModel {}
}