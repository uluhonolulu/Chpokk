using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Compilation;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Shouldly;

namespace Chpokk.Tests.Compilation {
	[TestFixture]
	public class SolutionCompilation: BaseCommandTest<SingleSolutionWithProjectFileContext> {
		[Test]
		public void Test() {


			File.Exists(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\CompileSolution\CompileSolution\bin\Debug\CompileSolution.exe").ShouldBe(true);
		}

		public override void Act() {
			var solutionPath =
				@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\CompileSolution\CompileSolution.sln";
			var logger = Context.Container.Get<ChpokkLogger>();
			logger.ConnectionId = "fakeID";
			var loggers = new ILogger[] { logger };
			var targets = new string[] { "Build" };
			var globalProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			var projectCollection = new ProjectCollection(globalProperties, loggers, null, ToolsetDefinitionLocations.ConfigurationFile | ToolsetDefinitionLocations.Registry, 1, false);
			var requestData = new BuildRequestData(solutionPath, globalProperties, null, targets, null);
			var parameters = new BuildParameters(projectCollection);
			parameters.ToolsetDefinitionLocations = ToolsetDefinitionLocations.ConfigurationFile | ToolsetDefinitionLocations.Registry;
			BuildManager.DefaultBuildManager.Build(parameters, requestData);			
		}
	}
}
