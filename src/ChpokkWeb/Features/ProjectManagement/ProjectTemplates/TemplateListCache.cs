using System;
using System.Collections.Generic;
using System.Linq;

namespace ChpokkWeb.Features.ProjectManagement.ProjectTemplates {
	public class TemplateListCache {
		private readonly Lazy<IEnumerable<ProjectTemplateData>> _templates;
		private readonly TemplateInstaller _templateInstaller;
		public TemplateListCache(TemplateInstaller templateInstaller) {
			_templateInstaller = templateInstaller;
			_templates = new Lazy<IEnumerable<ProjectTemplateData>>(GetProjectTemplates);

		}

		private IEnumerable<ProjectTemplateData> GetProjectTemplates() {
			return _templateInstaller.GetTemplateData();
		}

		public IEnumerable<ProjectTemplateData> ProjectTemplates {
			get { return _templates.Value; }
		}
	}
}