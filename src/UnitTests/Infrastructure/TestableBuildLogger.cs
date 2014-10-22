using System;
using ChpokkWeb.Features.Compilation;

namespace UnitTests.Infrastructure {
	public class TestableBuildLogger: ChpokkLogger {
		public string Log { get; private set; }
		public TestableBuildLogger() {
			Log = string.Empty;
		}

		public override void SendMessage(string text, MessageType type = MessageType.Info, bool wrap = true) {
			Log += text;
			if (wrap) {
				Log += Environment.NewLine;
			}
			Console.WriteLine(text);
		}

		public override void SendError(Microsoft.Build.Framework.BuildErrorEventArgs args) {
			Console.WriteLine("COMPILATION ERROR: " + args.Message);
		}
	}
}
