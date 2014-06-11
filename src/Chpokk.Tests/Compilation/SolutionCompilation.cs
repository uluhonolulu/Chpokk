using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Compilation;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Shouldly;
using FubuCore;

namespace Chpokk.Tests.Compilation {
	[TestFixture]
	public class SolutionCompilation: BaseCommandTest<SingleSolutionWithProjectFileContext> {
		[Test]
		public void Test() {
			var outputPath = Context.ProjectFolder.AppendPath(@"bin\Debug").AppendPath(Context.PROJECT_NAME + ".exe");
			//TODO: modify the context so that the project is buildable
			File.Exists(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\CompileSolution\CompileSolution\bin\Debug\CompileSolution.exe").ShouldBe(true);
		}

		public override void Act() {
			var solutionPath =
				@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\CompileSolution\CompileSolution.sln";
			solutionPath = Context.SolutionPath;
			var logger = Context.Container.Get<ChpokkLogger>();
			logger.ConnectionId = "fakeID";
			var loggers = new ILogger[] { logger };
			var targets = new string[] { "Build" };
			var globalProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			var projectCollection = new ProjectCollection(globalProperties, loggers, null, ToolsetDefinitionLocations.ConfigurationFile | ToolsetDefinitionLocations.Registry, 1, false);
			//var projekt =
			//	ProjectCollection.GlobalProjectCollection.LoadProject(
			//		@"D:\Projects\Chpokk\src\Chpokk.Tests\Chpokk.Tests.csproj");
			//projectCollection = ProjectCollection.GlobalProjectCollection;
			//projectCollection.RegisterLogger(logger);
			var project =
				ProjectRootElement.Open(
					Context.ProjectFilePath);
			if (project.DefaultTargets.IsEmpty()) {
				project.DefaultTargets = "Build";
				project.Save();
			}
			project.DefaultTargets.ShouldNotBe(string.Empty);
			var requestData = new BuildRequestData(solutionPath, globalProperties, null, targets, null);
			var parameters = new BuildParameters(projectCollection);
			parameters.ToolsetDefinitionLocations = ToolsetDefinitionLocations.ConfigurationFile | ToolsetDefinitionLocations.Registry;
			BuildManager.DefaultBuildManager.Build(parameters, requestData);			
		}

		public override void CleanUp() {
			base.CleanUp();
			DirectoryHelper.DeleteDirectory(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\CompileSolution\CompileSolution\bin\");
		}
	}
}
