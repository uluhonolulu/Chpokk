using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Authentication;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Security;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class BeginEditorExperienceEndpoint {
		private readonly InterestDetector _interestDetector;
		private readonly ISecurityContext _securityContext;
		private readonly ExperienceTracker _experienceTracker;
		private readonly UserManagerInContext _userManager;
		private ActivityTracker _activityTracker;
		public BeginEditorExperienceEndpoint(InterestDetector interestDetector, ISecurityContext securityContext, ExperienceTracker experienceTracker, UserManagerInContext userManager, ActivityTracker activityTracker) {
			_interestDetector = interestDetector;
			_securityContext = securityContext;
			_experienceTracker = experienceTracker;
			_userManager = userManager;
			_activityTracker = activityTracker;
		}

		public BeginEditorExperienceModel DoIt(BeginEditorExperienceDummyInputModel _) {
			_activityTracker.Record("Calling ShouldStart");
			var shouldStart = _interestDetector.ShouldStart(_securityContext.CurrentIdentity);
			_activityTracker.Record("Called ShouldStart");
			return new BeginEditorExperienceModel { ShouldStart = shouldStart };
		}

		public AjaxContinuation StartTiming(StartTimingInputModel model) {
			_experienceTracker.StartTracking(() =>
			{
				DisplayMessage(model.ConnectionId);
				//set the status to "canceled" -- if a user ignores the popup, it's like she's canceled
				SetStatusToCanceled();
			});
			return AjaxContinuation.Successful();
		}

		private void SetStatusToCanceled() {
			var currentUser = _userManager.GetCurrentUser();
			currentUser.Status = "canceled";
			_userManager.UpdateUser(currentUser);
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

//OH GOD OH GOD PLZ PLZ PLZ!!!