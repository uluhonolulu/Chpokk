using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ChpokkWeb.Features.Compilation;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Core.Ajax;
using Gallio.Runner;
using Gallio.Runtime;
using Gallio.Runtime.ConsoleSupport;
using Gallio.Runtime.Logging;
using Gallio.Runtime.ProgressMonitoring;
using Microsoft.AspNet.SignalR;

namespace ChpokkWeb.Features.Testing {
	public class TestingEndpoint {
		private readonly WebGallioConsole _webConsole;
		private readonly ChpokkLogger _logger;
		private readonly RepositoryManager _repositoryManager;
		private readonly MsBuildCompiler _compiler;
		private Tester _tester;
		public TestingEndpoint(WebGallioConsole webConsole, RepositoryManager repositoryManager, ChpokkLogger logger, MsBuildCompiler compiler, Tester tester) {
			_webConsole = webConsole;
			_repositoryManager = repositoryManager;
			_logger = logger;
			_compiler = compiler;
			_tester = tester;
		}

		public AjaxContinuation CompileAndTest(TestingInputModel model) {
			//TODO: Task
			_webConsole.ConnectionId = model.ConnectionId;
			_logger.ConnectionId = model.ConnectionId;
			_logger.RepositoryRoot = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName);
			var projectPath = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\TestingTest\TestingTest\TestingTest.csproj";
			var result = _compiler.Compile(projectPath, _logger);
			_tester.RunTheTests(_webConsole, new[]{result.OutputFilePath});
			return AjaxContinuation.Successful();
		}

		//public AjaxContinuation RunTests(TestingInputModel model) {
		//	Task.Run(() => {
		//		var webConsole = new WebGallioConsole{ConnectionId = model.ConnectionId};
		//		try {
		//			var logger = new FilteredLogger((ILogger)new RichConsoleLogger(webConsole), Verbosity.Normal);
		//			var setup = new RuntimeSetup();
		//			setup.AddPluginDirectory(@"C:\Program Files (x86)\Gallio\bin");
		//			if (!RuntimeAccessor.IsInitialized) {
		//				RuntimeBootstrap.Initialize(setup, logger);
		//			}
				
		//			var progressMonitorProvider = new RichConsoleProgressMonitorProvider(webConsole);
		//			var launcher = new TestLauncher { Logger = logger, ProgressMonitorProvider = progressMonitorProvider, RuntimeSetup = setup, EchoResults = true };
		//			launcher.TestProject.TestRunnerFactoryName = StandardTestRunnerFactoryNames.Local; 
		//			launcher.AddFilePattern(@"D:\Projects\Chpokk\src\ChpokkWeb\bin\SmokeTests.dll");
		//			var testLauncherResult = launcher.Run();
		//			webConsole.WriteLine(testLauncherResult.ResultSummary);
		//			//webConsole.WriteLine(testLauncherResult.Statistics.FormatTestCaseResultSummary());
		//		}
		//		catch (Exception e) {
		//			webConsole.WriteLine(e.ToString());
		//		}
		//	});
		//	return AjaxContinuation.Successful();
		//}
	}

	public class TestingInputModel {
		public string ConnectionId { get; set; }

		public string RepositoryName { get; set; }
	}

	public class WebGallioConsole : IRichConsole {
		private readonly IHubContext _hubContext = GlobalHost.ConnectionManager.GetHubContext<TestingHub>();

		public WebGallioConsole() {
			SyncRoot = new object();
			Width = 80;
			Out = Console.Out;
			Error = Console.Error;
		}
		public void ResetColor() { }
		public void SetFooter(Gallio.Common.Action showFooter, Gallio.Common.Action hideFooter) {}
		public void SetFooter(Action showFooter, Action hideFooter) { }
		public void Write(char c) { }
		public void Write(string str) { }
		public void WriteLine() { this.WriteLine(string.Empty); }
		public void WriteLine(string str) {
			Client.log(str);
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
		public string ConnectionId { get; set; }

		private dynamic Client {
			get { return _hubContext.Clients.Client(ConnectionId); }
		}
	}	
	
}