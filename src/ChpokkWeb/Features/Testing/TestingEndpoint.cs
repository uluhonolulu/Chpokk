using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ChpokkWeb.Features.Compilation;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Core.Ajax;
using Gallio.Runner;
using Gallio.Runtime;
using Gallio.Runtime.Logging;
using Gallio.Runtime.ProgressMonitoring;
using BuildResult = Microsoft.Build.Execution.BuildResult;

namespace ChpokkWeb.Features.Testing {
	public class TestingEndpoint {
		private readonly WebGallioConsole _webConsole;
		private readonly ChpokkLogger _logger;
		private readonly RepositoryManager _repositoryManager;
		private readonly SolutionExplorer _solutionExplorer;
		private readonly MsBuildCompiler _compiler;
		private SolutionCompiler _solutionCompiler;
		private readonly Tester _tester;
		public TestingEndpoint(WebGallioConsole webConsole, RepositoryManager repositoryManager, ChpokkLogger logger, MsBuildCompiler compiler, Tester tester, SolutionExplorer solutionExplorer, SolutionCompiler solutionCompiler) {
			_webConsole = webConsole;
			_repositoryManager = repositoryManager;
			_logger = logger;
			_compiler = compiler;
			_tester = tester;
			_solutionExplorer = solutionExplorer;
			_solutionCompiler = solutionCompiler;
		}

		public AjaxContinuation CompileAndTest(TestingInputModel model) {
			_webConsole.ConnectionId = model.ConnectionId;
			_logger.ConnectionId = model.ConnectionId;
			_logger.RepositoryRoot = _repositoryManager.GetAbsoluteRepositoryPath(model.RepositoryName);
			Task.Run(() => {
				//build
				var assemblyPaths = new List<string>();
				foreach (var solutionPath in GetSolutionPaths(model.RepositoryName)) {
					var buildResult = _solutionCompiler.CompileSolution(solutionPath, _logger);
					assemblyPaths.AddRange(GetAssemblyPaths(buildResult));
				}
				//test
				_tester.RunTheTests(_webConsole, assemblyPaths);
			});
			return AjaxContinuation.Successful();

		}

		private IEnumerable<string> GetAssemblyPaths(BuildResult buildResult) {
			return from result in buildResult.ResultsByTarget["Build"].Items select result.ItemSpec;
		} 

		private IEnumerable<string> GetSolutionPaths(string repositoryName) {
			var repositoryRoot = _repositoryManager.GetAbsoluteRepositoryPath(repositoryName);
			return _solutionExplorer.GetSolutionFiles(repositoryRoot);
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
}