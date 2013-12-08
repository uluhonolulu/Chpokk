using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.ProjectManagement.AddSimpleProject;

namespace ChpokkWeb.Features.ProjectManagement.AddProject {
	public class AddProjectInputModel: AddSimpleProjectInputModel {
		public string SolutionPath { get; set; }
		public string ProjectName { get; set; }
	}
}