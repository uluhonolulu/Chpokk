using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Build.Evaluation;

namespace ChpokkWeb.Features.Compilation {
	static class ProjectBuildSync {
		private static readonly object _syncObject = new object();
		public static bool Build(Project project) {
			lock (_syncObject)
				return project.Build();
		}
	}

}