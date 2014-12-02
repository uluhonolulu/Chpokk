using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChpokkWeb.Infrastructure.Logging {
	public class SimpleLogger {

		public static SimpleLogger CreateLogger(string connectionId) {
			return new SimpleLogger(connectionId);
		}

		public void Log(string message) {
			this.Client.log(message);
		}

		private SimpleLogger(string connectionId) {
			ConnectionId = connectionId;
		}


		private readonly IHubContext _hubContext = GlobalHost.ConnectionManager.GetHubContext<SimpleLoggerHub>();

		private dynamic Client {
			get {
				EnsureConnection();
				return _hubContext.Clients.Client(ConnectionId);
			}
		}

		private string ConnectionId { get; set; }
		private void EnsureConnection() {
			if (ConnectionId.IsEmpty()) throw new InvalidOperationException("Can't write to an unknown connection");
		}


	}

	public class SimpleLoggerHub : Hub {}

}