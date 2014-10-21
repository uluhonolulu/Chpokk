using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;

namespace Chpokk.Tests.Infrastructure {
	public class TestableNuGetLogger: SignalRLogger {
		public override void Write(string value) {
			//base.Write(value);
		}
		public override void WriteLine(string value) {
			//base.WriteLine(value);
		}
		protected override void WriteColor(System.IO.TextWriter textWriter, ConsoleColor consoleColor, string format, object[] args) {
			//base.WriteColor(textWriter, consoleColor, format, args);
		}
	}
}
