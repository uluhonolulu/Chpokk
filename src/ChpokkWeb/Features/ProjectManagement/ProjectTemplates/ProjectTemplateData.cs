using System.Collections.Generic;
using System.Linq;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.ProjectTemplates {
	public class ProjectTemplateData {
		public string Name { get; set; }
		public string RequiredFrameworkVersion { get; set; }
		public string Path { get; set; }
		public string DisplayPath {
			get {
				var ignoredFolders = new[] { "1033", "Version2012" };
				return Path.ParentDirectory().ParentDirectory().getPathParts().Except(ignoredFolders).Join(@"\");
			}
		}
	}
}