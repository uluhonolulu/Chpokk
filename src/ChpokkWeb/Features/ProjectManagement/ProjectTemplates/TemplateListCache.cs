using System;
using System.Collections.Generic;
using System.Linq;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.ProjectTemplates {
	public class TemplateListCache {
		private Lazy<IEnumerable<ProjectTemplateData>> _templates;
		private readonly IAppRootProvider _rootProvider;
		private readonly FileSystem _fileSystem;
		public TemplateListCache(IAppRootProvider rootProvider, FileSystem fileSystem) {
			_rootProvider = rootProvider;
			_fileSystem = fileSystem;
			_templates = new Lazy<IEnumerable<ProjectTemplateData>>(GetProjectTemplates);

		}

		private IEnumerable<ProjectTemplateData> GetProjectTemplates() {
			var templateFolder = _rootProvider.AppRoot.AppendPath(@"SystemFiles\Templates\ProjectTemplates\");
			var templateFiles = _fileSystem.FindFiles(templateFolder, new FileSet() { Include = "*.vstemplate", DeepSearch = true });
			var templates = from file in templateFiles select Template.LoadTemplate(file);
			return from template in templates
								select new ProjectTemplateData() {
									Name = template.Name,
									Path = template.Path.PathRelativeTo(templateFolder),
									RequiredFrameworkVersion = template.RequiredFrameworkVersion
								};
		}

		public IEnumerable<ProjectTemplateData> ProjectTemplates {
			get { return _templates.Value; }
		}
	}
}