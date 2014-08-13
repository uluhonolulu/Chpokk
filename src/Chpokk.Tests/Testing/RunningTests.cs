using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Testing;
using FubuMVC.Core.Urls;
using Gallio.Common.Diagnostics;
using Gallio.Framework;
using Gallio.Model;
using Gallio.Runner;
using Gallio.Runner.Events;
using Gallio.Runtime;
using Gallio.Runtime.ConsoleSupport;
using Gallio.Runtime.Logging;
using Gallio.Runtime.ProgressMonitoring;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Action = Gallio.Common.Action;
using Ivonna.Framework.Generic;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class RunningTests: BaseCommandTest<SimpleConfiguredContext> {
		[Test]
		public void OutputShouldContainFailedTests() {
			var webConsole = Context.Container.Get<WebConsole>();
			webConsole.Log.ShouldContain(s => s.StartsWith("[failed] Test SomeTests/FailingTests/JustFails")); 
		}

		[Test]
		public void OutputShouldContainPassedTests() {
			var webConsole = Context.Container.Get<WebConsole>();
			webConsole.Log.ShouldContain(s => s.StartsWith("[passed] Test SomeTests/PassingTest/ImEmpty")); //should contain passed tests
		}

		public override void Act() {
			var webConsole = Context.Container.Get<WebConsole>();
			var logger = new FilteredLogger((ILogger)new RichConsoleLogger(webConsole), Verbosity.Verbose); //changing it to Normal displays failed tests; verbose displays passed as well
			var setup = new RuntimeSetup();
			setup.AddPluginDirectory(@"C:\Program Files (x86)\Gallio\bin");
			if (!RuntimeAccessor.IsInitialized) {
				RuntimeBootstrap.Initialize(setup, logger);
			} 
			var progressMonitorProvider = new RichConsoleProgressMonitorProvider(webConsole);
			var launcher = new TestLauncher {Logger = logger, ProgressMonitorProvider = progressMonitorProvider, EchoResults = true};
			launcher.TestProject.TestRunnerFactoryName = StandardTestRunnerFactoryNames.Local;
			//launcher.AddFilePattern(@"D:\Projects\Chpokk\src\ChpokkWeb\bin\SmokeTests.dll");
			launcher.AddFilePattern(@"D:\Projects\Chpokk\src\SomeTests\bin\Debug\SomeTests.dll");
			var testLauncherResult = launcher.Run();
			webConsole.WriteLine(testLauncherResult.ResultSummary);
			Console.WriteLine();
			Console.WriteLine("Here's the log:");
			webConsole.Log.Each((s, i) => Console.WriteLine(i.ToString() + ". " + s));			
		}
	}

		public class WebConsole: IRichConsole {
			public WebConsole() { 
				SyncRoot = new object();
				Width = 80;
				Out = Console.Out;
				//Error = Console.Error;
			}
			public void ResetColor() {}
			public void SetFooter(Action showFooter, Action hideFooter) {}
			public void Write(char c) {}
			public void Write(string str) {}
			public void WriteLine() {
				Console.WriteLine();
			}
			public void WriteLine(string str) {
				Console.WriteLine(str);
				Log.Add(str + " -- " + ForegroundColor);
			}
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

			public IList<string> Log = new List<string>();
		}	
	

}
