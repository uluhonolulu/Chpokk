using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CThru;
using CThru.BuiltInAspects;
using Gallio.Framework;
using Gallio.Runner;
using Gallio.Runtime.ConsoleSupport;
using Gallio.Runtime.Logging;
using Gallio.Runtime.ProgressMonitoring;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Action = Gallio.Common.Action;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class RunningTests {
		[Test]
		public void Test() {
			CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IRichConsole && info.MethodName.StartsWith("Write")));// || info.TargetInstance is ILogger
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is ILogger));// || info.TargetInstance is ILogger
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IProgressMonitor));// || info.TargetInstance is ILogger
			CThruEngine.StartListening();
			var webConsole = new WebConsole();	
			var logger = new FilteredLogger((ILogger)new RichConsoleLogger(webConsole), Verbosity.Normal);
			var progressMonitorProvider = new RichConsoleProgressMonitorProvider(webConsole);
			var launcher = new TestLauncher {Logger = logger, ProgressMonitorProvider = progressMonitorProvider, EchoResults = true};
			launcher.AddFilePattern(@"D:\Projects\Chpokk\src\ChpokkWeb\bin\SmokeTests.dll");
			var testLauncherResult = launcher.Run();
			//Must use Gallio.Runner.TestLauncher
			//Look at EchoProgram.RunTests(ILogger logger)
			//
			//gallio.models.helpers.simpletestdriver.runassembly
			//Gallio.dll!Gallio.Framework.Pattern.PatternTestInstanceState.InvokeFixtureMethod(Gallio.Common.Reflection.IMethodInfo method = {Gallio.Common.Reflection.Impl.NativeMethodWrapper}, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePai...

		}

		public class WebConsole: IRichConsole {
			public WebConsole() { 
				SyncRoot = new object();
				Width = 80;
				Out = Console.Out;
				Error = Console.Error;
			}
			public void ResetColor() {}
			public void SetFooter(Action showFooter, Action hideFooter) {}
			public void Write(char c) {}
			public void Write(string str) {}
			public void WriteLine() {}
			public void WriteLine(string str) {}
			public object SyncRoot { get; private set; }
			public bool IsCancelationEnabled { get; set; }
			public bool IsCanceled { get; set; }
			public bool IsRedirected { get; private set; }
			public TextWriter Error { get; private set; }
			public TextWriter Out { get; private set; }
			public ConsoleColor ForegroundColor { get; set; }
			public int CursorLeft { get; set; }
			public int CursorTop { get; set; }
			public string Title { get; set; }
			public int Width { get; private set; }
			public bool FooterVisible { get; set; }
			public event EventHandler Cancel;
		}
	}
}
