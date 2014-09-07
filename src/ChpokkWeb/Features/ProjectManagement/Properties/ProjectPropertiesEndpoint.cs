using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.ProjectManagement.Properties {
	public class ProjectPropertiesEndpoint {
		public ProjectPropertiesModel DoIt(ProjectPropertiesInputModel model) {
			var output = new ProjectPropertiesModel();
			output.BclReferences.Add("System", false);
			output.BclReferences.Add("System.Core", true);
			output.PackageReferences.Add("Autofac");
			return output;
		}
	}

	public class ProjectPropertiesModel {
		public IDictionary<string, bool> BclReferences = new Dictionary<string, bool>();
		public IList<string> PackageReferences = new List<string>();
		public IDictionary<string, bool> ProjectReferences = new Dictionary<string, bool>();
	}

	public class ProjectPropertiesInputModel {}
}