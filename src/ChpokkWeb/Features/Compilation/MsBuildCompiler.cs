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

		public bool Compile(string projectFilePath) {
			_projectCollection.RegisterLogger(_logger);
			try {
				var project = _projectCollection.LoadProject(projectFilePath);
				return project.Build();
			}
			finally {
				_projectCollection.UnregisterAllLoggers();
			}
		}
	}
}