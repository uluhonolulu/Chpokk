using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.ProjectTemplates {
	public class TemplateListEndpoint {
		public TemplateListModel DoIt() {
			var template =
				Template.LoadTemplate(
					@"D:\Projects\Chpokk\src\ChpokkWeb\SystemFiles\Templates\ProjectTemplates\CSharp\WCF\1033\RssService\RssServiceLibrary.vstemplate");
			var templateData = new ProjectTemplateData()
				{
					Name = template.Name,
					Path = @"CSharp\WCF\1033\RssService\RssServiceLibrary.vstemplate",
					RequiredFrameworkVersion = template.RequiredFrameworkVersion
				};
			var template2 =
				Template.LoadTemplate(
					@"D:\Projects\Chpokk\src\ChpokkWeb\SystemFiles\Templates\ProjectTemplates\CSharp\WCF\1033\SequentialWorkflowServiceLibrary\SequentialWorkflowServiceLibrary.vstemplate");
			var templateData2 = new ProjectTemplateData()
				{
					Name = template2.Name,
					Path = @"CSharp\WCF\1033\SequentialWorkflowServiceLibrary\SequentialWorkflowServiceLibrary.vstemplate",
					RequiredFrameworkVersion = template2.RequiredFrameworkVersion
				};
			return new TemplateListModel() {Templates = new[] {templateData, templateData2}};
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