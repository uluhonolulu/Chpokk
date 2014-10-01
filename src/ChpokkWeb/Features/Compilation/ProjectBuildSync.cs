using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

namespace ChpokkWeb.Features.Compilation {
	static class ProjectBuildSync {
		private static readonly object _syncObject = new object();
		public static bool Build(Project project, ILogger logger) {
			lock (_syncObject)
				return project.Build(logger);
		}

		public static Microsoft.Build.Execution.BuildResult Build(BuildParameters parameters, BuildRequestData requestData) {
			lock (_syncObject)
				return BuildManager.DefaultBuildManager.Build(parameters, requestData);	
		}
	}

}