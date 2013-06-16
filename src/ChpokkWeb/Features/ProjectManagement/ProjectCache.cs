using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore.Util;

namespace ChpokkWeb.Features.ProjectManagement {
	public class ProjectCache: Cache<string, ProjectData> {
		public ProjectCache() : base() {}
	}
}