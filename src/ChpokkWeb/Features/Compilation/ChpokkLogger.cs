using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNet.SignalR;
using Microsoft.Build.Framework;
using System.Linq;
using FubuCore;

namespace ChpokkWeb.Features.Compilation {
	public class ChpokkLogger: ILogger {
		private readonly IHubContext _hubContext;

		public ChpokkLogger() {
			Events = new List<BuildEventArgs>();
			_hubContext = GlobalHost.ConnectionManager.GetHubContext<CompilerHub>();
		}

		public List<BuildEventArgs> Events { get; set; }


		public enum MessageType {
			Info, Error, Success
		}

		public void SendMessage(string text, MessageType type = MessageType.Info, bool wrap = true) {
			switch (type) {
				case MessageType.Success:
					Client.success(text, wrap);
					break;
				case MessageType.Info:
					Client.info(text, wrap);
					break;
				case MessageType.Error:
					Client.danger(text, wrap);
					break;
			}
		}

		private dynamic Client {
			get {
				EnsureConnectionId();
				return _hubContext.Clients.Client(ConnectionId);
			}
		}
		private void EnsureConnectionId() {
			if (ConnectionId.IsEmpty()) throw new InvalidOperationException("Can't write to an unknown connection");
		}

		public string ConnectionId { get; set; }


		public void Initialize(IEventSource eventSource) {
			EnsureConnectionId();
			Verbosity = LoggerVerbosity.Quiet;
			eventSource.BuildStarted += (sender, args) => SendMessage(args.Message);
			eventSource.ProjectStarted += (sender, args) => SendMessage(args.Message); // much more info here
			eventSource.ProjectFinished += (sender, args) => {
				var messageType = args.Succeeded ? MessageType.Success : MessageType.Error;
				SendMessage(args.Message, messageType);
			};
			eventSource.MessageRaised += (sender, args) => {
				if (args.Importance > MessageImportance.Normal) {
					SendMessage(args.Message);
				}
			};
			eventSource.ErrorRaised += (sender, args) => SendMessage(args.Message + ": " + args.File + ", line " + args.LineNumber + ", position " + args.ColumnNumber, MessageType.Error);
			eventSource.BuildFinished += (sender, args) => {
				var messageType = args.Succeeded ? MessageType.Success : MessageType.Error;
				SendMessage(args.Message, messageType);
			};
		}
		public void Shutdown() { }
		public LoggerVerbosity Verbosity { get; set; }
		public string Parameters { get; set; }

	}
}