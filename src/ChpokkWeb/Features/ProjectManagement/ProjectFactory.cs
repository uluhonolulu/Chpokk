using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.ProjectManagement {
	public class ProjectFactory {
		public ProjectData GetProjectData(string key) {
			return new ProjectData();
		}
	}
}