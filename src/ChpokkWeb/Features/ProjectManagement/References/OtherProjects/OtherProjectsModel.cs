using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;

namespace ChpokkWeb.Features.ProjectManagement.References.OtherProjects {
	public class OtherProjectsModel {
		public IEnumerable<ProjectItem> Projects { get; set; }
	}

	public class OtherProjectsInputModel : BaseRepositoryInputModel {
		public string SolutionPath { get; set; }
	}
}