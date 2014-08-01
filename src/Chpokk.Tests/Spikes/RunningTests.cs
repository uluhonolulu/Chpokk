using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
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

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb]
	public class RunningTestsWebTest: WebCommandTest<SimpleAuthenticatedContext> {
		private Spy<string> _consoleSpy;

		[Test]
		public void OutputShouldIncludeTestResults() {
			
		}
		//[RunOnWeb]
		public override void Act() {
			_consoleSpy = new Spy<string>(info => info.TargetInstance is IRichConsole && info.MethodName.StartsWith("Write"), args => args.ParameterValues[0] as string);
			CThruEngine.AddAspect(_consoleSpy);

			var session = new TestSession();
			var urlRegistry = Context.Container.Get<IUrlRegistry>();
			var url = urlRegistry.UrlFor<ChpokkWeb.Features.Testing.TestingInputModel>();
			session.Post(url, new TestingInputModel{ConnectionId = "-"});
			Thread.Sleep(10000);
			var count = _consoleSpy.Results.Count();
			Console.WriteLine(count);
		}
	}

	[TestFixture]
	public class RunningTests {
		[Test]
		public void Test() {
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IRichConsole && info.MethodName == "WriteLine", 10));// || info.TargetInstance is ILogger
			//CThruEngine.AddAspect(new TraceAspect(info => info.MethodName == "NotifyTestStepFinished")
			//	.DisplayFor<TestStepFinishedEventArgs>(args => args.GetStepKind() + ": " + args.Test + ", " + args.TestStepRun));// || info.TargetInstance is ILogger
			CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is BaseTestDriver));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IRichConsole && info.MethodName.Contains("Color")));// || info.TargetInstance is ILogger
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is FilteredLogger));// || info.TargetInstance is ILogger
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IProgressMonitor && info.MethodName == "SetStatus"));// || info.TargetInstance is ILogger
			CThruEngine.StartListening();
			var webConsole = new WebConsole();	
			var logger = new FilteredLogger((ILogger)new RichConsoleLogger(webConsole), Verbosity.Verbose); //changing it to Normal displays failed tests; verbose displays passed as well
			var setup = new RuntimeSetup();
			setup.AddPluginDirectory(@"C:\Program Files (x86)\Gallio\bin");
			if (!RuntimeAccessor.IsInitialized) {
				RuntimeBootstrap.Initialize(setup, logger);
			} 
			var progressMonitorProvider = new RichConsoleProgressMonitorProvider(webConsole);
			var launcher = new TestLauncher {Logger = logger, ProgressMonitorProvider = progressMonitorProvider, EchoResults = true};
			//with local, no detailed output
			//with IsolatedProcess, it works just fine
			launcher.TestProject.TestRunnerFactoryName = StandardTestRunnerFactoryNames.Local;
			launcher.AddFilePattern(@"D:\Projects\Chpokk\src\ChpokkWeb\bin\SmokeTests.dll");
			launcher.AddFilePattern(@"D:\Projects\Chpokk\src\SomeTests\bin\Debug\SomeTests.dll");
			var testLauncherResult = launcher.Run();
			Console.WriteLine(testLauncherResult.ResultSummary);
			webConsole.WriteLine(testLauncherResult.Statistics.FormatTestCaseResultSummary());
			//Must use Gallio.Runner.TestLauncher
			//Look at EchoProgram.RunTests(ILogger logger)
			//
			//gallio.models.helpers.simpletestdriver.runassembly
			//Gallio.dll!Gallio.Framework.Pattern.PatternTestInstanceState.InvokeFixtureMethod(Gallio.Common.Reflection.IMethodInfo method = {Gallio.Common.Reflection.Impl.NativeMethodWrapper}, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePai...
			//var testLauncher = new Gallio.Runner.TestLauncher(){Logger = new ConsoleLogger(), ProgressMonitorProvider = new LogProgressMonitorProvider(new ConsoleLogger())};
			//testLauncher.AddFilePattern(@"D:\Projects\libgit2sharp\LibGit2Sharp.Tests\bin\Release\LibGit2Sharp.Tests.dll");
			//testLauncher.EchoResults = true;
			////testLauncher.ProgressMonitorProvider = new ProgressProvider();
			//testLauncher.TestProject.TestRunnerFactoryName = StandardTestRunnerFactoryNames.Local;
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IProgressMonitor || info.TargetInstance is IProgressMonitorPresenter || info.TargetInstance is IProgressMonitorProvider));
			////CThruEngine.StartListening();
			//var result = testLauncher.Run();
			//Console.WriteLine(result.ResultSummary);
			//Console.WriteLine(result.Statistics.FormatTestCaseResultSummary());

			//also look into:
			//Gallio.Runner.Extensions.LogExtension -- should create with calling Install(ITestRunnerEvents events, ILogger logger)
			//registered when testLauncher.EchoResults = true; (but what is the logger?)
			//gotta track the logger (all calls to ILogger)
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
		}	
	

}
