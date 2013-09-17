using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Build.Evaluation;

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
				var outputProperty = project.AllEvaluatedProperties.First(property => property.Name == "TargetPath");
				var outputFilePath = outputProperty.EvaluatedValue;

				var buildResult = project.Build();
				return new BuildResult{Success = buildResult, OutputFilePath = outputFilePath};
			}
			finally {
				_projectCollection.UnregisterAllLoggers();
			}
		}
	}

	public class BuildResult {
		public bool Success { get; set; }
		public string OutputFilePath { get; set; }
	}
}