using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using FubuMVC.Core.Ajax;
using Microsoft.AspNet.SignalR;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet.Spike {
	public class LongOpEndpoint {
		public AjaxContinuation LongOp(LongOpInputModel input) {
			var hubContext = GlobalHost.ConnectionManager.GetHubContext<LongOpHub>();
			for (int i = 0; i < 10; i++) {
				Thread.Sleep(1000);
				hubContext.Clients.Client(input.ConnectionId).sendMessage(input.ConnectionId);
			}
			return AjaxContinuation.Successful();
		}
	}
}