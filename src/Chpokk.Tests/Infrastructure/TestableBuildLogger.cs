using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChpokkWeb.Features.Compilation;

namespace Chpokk.Tests.Infrastructure {
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
		}
	}
}
