using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ChpokkWeb.Features.CustomerDevelopment.TrialSignup {
	public class UserHub: Hub {
		public void DisplayTrialInvitation() {
			Clients.All.displayTrialInvitation();
		}
	}
}