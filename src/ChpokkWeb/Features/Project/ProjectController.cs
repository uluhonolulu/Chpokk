using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core;

namespace ChpokkWeb.Features.Project {
	public class ProjectController {
		[UrlPattern("Project/{Name}")]
		public ProjectModel Get(ProjectInputModel input) {
			return new ProjectModel();
		}
	}
}