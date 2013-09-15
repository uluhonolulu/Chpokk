using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using System.Linq;
using FubuCore;

namespace ChpokkWeb.Features.Compilation {
	public class ChpokkLogger: ILogger {
		public ChpokkLogger() {
			Events = new List<BuildEventArgs>();
			Verbosity = LoggerVerbosity.Minimal;
		}

		public List<BuildEventArgs> Events { get; set; }

		public void Initialize(IEventSource eventSource) {
			eventSource.AnyEventRaised += (sender, args) => Events.Add(args);
		}
		public void Shutdown() {}
		public LoggerVerbosity Verbosity { get; set; }
		public string Parameters { get; set; }

		public string GetLogMessage() {
			var builder = new StringBuilder();
			foreach (var @event in Events.OfType<BuildErrorEventArgs>()) {
				var message = "Error: {0} ({1}, line {2})".ToFormat(@event.Message, @event.File, @event.LineNumber);
				builder.AppendLine(message);
			}
			foreach (var @event in Events.OfType<ProjectFinishedEventArgs>()) {
				builder.AppendLine(@event.Message);
			}
			foreach (var @event in Events.OfType<BuildFinishedEventArgs>()) {
				//builder.AppendLine(@event.Message);
			}
			return builder.ToString();
		}
	}
}