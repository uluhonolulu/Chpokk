using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using Microsoft.Build.Evaluation;
using FubuCore;
using Microsoft.Build.Framework;

namespace ChpokkWeb.Features.Compilation {
	public class MsBuildCompiler {
		private readonly ProjectCollection _projectCollection;
		private readonly IAppRootProvider _rootProvider;

		public MsBuildCompiler(ProjectCollection projectCollection, IAppRootProvider rootProvider) {
			_projectCollection = projectCollection;
			_rootProvider = rootProvider;
			//_projectCollection.RegisterLogger(logger);
		}

		public BuildResult Compile(string projectFilePath, ILogger logger) {
			var customProperties = new Dictionary<string, string>()
				{
					{"VSToolsPath", _rootProvider.AppRoot.AppendPath(@"SystemFiles\Targets") }
				};
			var project = _projectCollection.LoadProject(projectFilePath, customProperties, null);
			var imports = project.Imports.Select(import => import.ImportedProject.FullPath);
			Console.WriteLine("IMPORTS");
			foreach (var import in imports) {
				Console.WriteLine(import + ": " + File.Exists(import));
			}
			var importPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\Microsoft.CSharp.targets";
			Console.WriteLine(importPath + ": " + File.Exists(importPath));
			//Console.WriteLine("PROPS");
			//foreach (var property in project.AllEvaluatedProperties) {
			//	Console.WriteLine(property.Name + ": " + property.EvaluatedValue);
			//}
			var buildResult = ProjectBuildSync.Build(project, logger);
			var outputPathProperty = project.AllEvaluatedProperties.First(property => property.Name == "OutputPath");
			var targetProperty = project.AllEvaluatedProperties.First(property => property.Name == "TargetFileName");
			var outputTypeProperty = project.AllEvaluatedProperties.First(property => property.Name == "OutputType");
			var outputFilePath = Path.Combine(projectFilePath.ParentDirectory(), outputPathProperty.EvaluatedValue,
				                                targetProperty.EvaluatedValue);
			var outputType = outputTypeProperty.EvaluatedValue;

			return new BuildResult{Success = buildResult, OutputFilePath = outputFilePath, OutputType = outputType};
		}
	}

	public class BuildResult {
		public bool Success { get; set; }
		public string OutputFilePath { get; set; }
		public string OutputType { get; set; }
	}
}