using ChpokkWeb.Infrastructure;
using FubuCore;
using Gallio.Runner;
using Gallio.Runtime;
using Gallio.Runtime.ConsoleSupport;
using Gallio.Runtime.Logging;
using Gallio.Runtime.ProgressMonitoring;

namespace ChpokkWeb.Features.Testing {
	public class Tester {
		private readonly IAppRootProvider _rootProvider;
		public Tester(IAppRootProvider rootProvider) {
			_rootProvider = rootProvider;
		}

		public void RunTheTests(IRichConsole webConsole, string[] testAssemblies) {
			var logger = new FilteredLogger(new RichConsoleLogger(webConsole), Verbosity.Verbose);//changing it to Normal displays failed tests; verbose displays passed as well
			if (!RuntimeAccessor.IsInitialized) {
				var setup = new RuntimeSetup();
				setup.AddPluginDirectory(_rootProvider.AppRoot.AppendPath(@"SystemFiles\GallioPlugins"));
				RuntimeBootstrap.Initialize(setup, logger);
			}
			var progressMonitorProvider = new RichConsoleProgressMonitorProvider(webConsole);
			var launcher = new TestLauncher
				{
					Logger = logger,
					ProgressMonitorProvider = progressMonitorProvider,
					EchoResults = true,
					TestProject = {TestRunnerFactoryName = StandardTestRunnerFactoryNames.Local}
				};
			foreach (var assembly in testAssemblies) launcher.AddFilePattern(assembly);

			var testLauncherResult = launcher.Run();
			webConsole.WriteLine(testLauncherResult.ResultSummary);
		}
	}
}