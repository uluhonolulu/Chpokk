using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace ChpokkWeb.Features.Compilation {
	public class ChpokkLogger: ILogger {
		public ChpokkLogger() {
			Messages = new List<string>();
			Events = new List<BuildEventArgs>();
		}

		public List<string> Messages { get; set; }
		public List<BuildEventArgs> Events { get; set; }

		public void Initialize(IEventSource eventSource) {
			eventSource.AnyEventRaised += (sender, args) => Events.Add(args);
			eventSource.MessageRaised += (sender, args) =>
			{
				if (args.Importance == MessageImportance.High) {
					Messages.Add(args.Message);
				}
				
			};
			eventSource.ProjectFinished += (sender, args) => Messages.Add(args.Message);
			eventSource.ErrorRaised += (sender, args) => Messages.Add("Error: " + args.Message);
		}
		public void Shutdown() {}
		public LoggerVerbosity Verbosity { get; set; }
		public string Parameters { get; set; }
	}
}