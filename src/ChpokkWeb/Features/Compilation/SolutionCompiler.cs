using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

namespace ChpokkWeb.Features.Compilation {
	public class SolutionCompiler {
		public Microsoft.Build.Execution.BuildResult CompileSolution(string solutionPath, ILogger logger) {
			var loggers = new[] { logger }; 
			var targets = new[] { "Build" };
			var globalProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			var projectCollection = new ProjectCollection(globalProperties, loggers, null, ToolsetDefinitionLocations.ConfigurationFile | ToolsetDefinitionLocations.Registry, 1, false);
			//var project =
			//	ProjectRootElement.Open(
			//		@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\_newCompilableSolution\_newCompilableSolution\_newCompilableSolution.csproj");
			//if (project.DefaultTargets.IsEmpty()) {
			//	project.DefaultTargets = "Build";
			//	project.Save();
			//}
			var requestData = new BuildRequestData(solutionPath, globalProperties, null, targets, null);
			var parameters = new BuildParameters(projectCollection) {
				Loggers = loggers, //need this to have any output at all
				ToolsetDefinitionLocations = ToolsetDefinitionLocations.ConfigurationFile | ToolsetDefinitionLocations.Registry
			};
			return BuildManager.DefaultBuildManager.Build(parameters, requestData);				
		}
	}
}