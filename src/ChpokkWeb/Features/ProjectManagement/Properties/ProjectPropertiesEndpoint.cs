using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.ProjectManagement.Properties {
	public class ProjectPropertiesEndpoint {
		public ProjectPropertiesModel DoIt(ProjectPropertiesInputModel model) {
			var output = new ProjectPropertiesModel();
			output.BclReferences.Add(new {Name = "System", Selected = false});
			output.BclReferences.Add(new { Name = "System.Core", Selected = true });
			output.PackageReferences.Add("Autofac");
			output.ProjectName = "ProjectName";
			return output;
		}
	}

	public class ProjectPropertiesModel {
		public string ProjectName;
		public IList<object> BclReferences = new List<object>();
		public IList<string> PackageReferences = new List<string>();
		public IList<object> ProjectReferences = new List<object>();
	}

	public class ProjectPropertiesInputModel {}
}