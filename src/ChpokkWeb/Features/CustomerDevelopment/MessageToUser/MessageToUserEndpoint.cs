using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChpokkWeb.Features.CustomerDevelopment.MessageToUser {
	public class MessageToUserEndpoint {
		public string DisplayPublishWarning() {
			const string message = "I'm sorry, but I need to upgrade the application in 5 minutes. Please save your work.";
			var context = GlobalHost.ConnectionManager.GetHubContext<MessageToUserHub>();
			context.Clients.All.info(message);
			return "OK";
		}
	}
}