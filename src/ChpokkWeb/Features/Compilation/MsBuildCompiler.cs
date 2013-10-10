using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Build.Evaluation;
using FubuCore;

namespace ChpokkWeb.Features.Compilation {
	public class MsBuildCompiler {
		private readonly ProjectCollection _projectCollection;
		private readonly ChpokkLogger _logger;

		public MsBuildCompiler(ProjectCollection projectCollection, ChpokkLogger logger) {
			_projectCollection = projectCollection;
			_logger = logger;
		}

		public BuildResult Compile(string projectFilePath) {
			_projectCollection.RegisterLogger(_logger);
			try {
				var project = _projectCollection.LoadProject(projectFilePath);
				var outputPathProperty = project.AllEvaluatedProperties.First(property => property.Name == "OutputPath");
				var targetProperty = project.AllEvaluatedProperties.First(property => property.Name == "TargetFileName");
				var outputTypeProperty = project.AllEvaluatedProperties.First(property => property.Name == "OutputType");
				var outputFilePath = Path.Combine(projectFilePath.ParentDirectory(), outputPathProperty.EvaluatedValue,
				                                  targetProperty.EvaluatedValue);
				var outputType = outputTypeProperty.EvaluatedValue;

				var buildResult = project.Build();
				return new BuildResult{Success = buildResult, OutputFilePath = outputFilePath, OutputType = outputType};
			}
			finally {
				_projectCollection.UnregisterAllLoggers();
			}
		}
	}

	public class BuildResult {
		public bool Success { get; set; }
		public string OutputFilePath { get; set; }
		public string OutputType { get; set; }
	}
}