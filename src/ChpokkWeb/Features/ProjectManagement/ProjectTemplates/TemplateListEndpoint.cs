using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.ProjectTemplates {
	public class TemplateListEndpoint {
		private readonly IAppRootProvider _rootProvider;
		private readonly FileSystem _fileSystem;
		public TemplateListEndpoint(IAppRootProvider rootProvider, FileSystem fileSystem) {
			_rootProvider = rootProvider;
			_fileSystem = fileSystem;
		}

		public TemplateListModel DoIt() {
			var templateFolder = _rootProvider.AppRoot.AppendPath(@"SystemFiles\Templates\ProjectTemplates\");
			var templateFiles = _fileSystem.FindFiles(templateFolder, new FileSet() { Include = "*.vstemplate", DeepSearch = true });
			var templates = from file in templateFiles select Template.LoadTemplate(file);
			var templateItems = from template in templates select  new ProjectTemplateData()
				{
					Name = template.Name,
					Path = template.Path.PathRelativeTo(templateFolder),
					RequiredFrameworkVersion = template.RequiredFrameworkVersion
				};
			return new TemplateListModel() {Templates = templateItems.ToArray()};

		}
	}

	public class TemplateListModel {
		public ProjectTemplateData[] Templates { get; set; }
	}

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