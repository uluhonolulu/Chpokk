using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.ProjectTemplates;
using ChpokkWeb.Infrastructure;
using FubuCore;
using ICSharpCode.NRefactory;
using Microsoft.Build.Construction;

namespace ChpokkWeb.Features.ProjectManagement.AddProject {
	public class ProjectCreator {
		private readonly IFileSystem _fileSystem;
		private readonly ProjectParser _projectParser;
		private readonly IAppRootProvider _appRootProvider;
		private readonly TemplateTransformer _templateTransformer;
		private readonly TemplateInstaller _templateInstaller;
		public ProjectCreator(ProjectParser projectParser, IFileSystem fileSystem, IAppRootProvider appRootProvider, TemplateTransformer templateTransformer, TemplateInstaller templateInstaller) {
			_projectParser = projectParser;
			_fileSystem = fileSystem;
			_appRootProvider = appRootProvider;
			_templateTransformer = templateTransformer;
			_templateInstaller = templateInstaller;
		}

		public ProjectRootElement CreateProject(string outputType, string projectName, string projectPath, string templatePath,
											 SupportedLanguage language) {
			if (outputType == "Web" || outputType == "Template") {
				// from template
				if (outputType == "Web")
					templatePath = @"{0}\Web\Version2012\1033\EmptyWebApplicationProject40\EmptyWebApplicationProject40.vstemplate".ToFormat(language == SupportedLanguage.CSharp ? "CSharp" : "VisualBasic");
				_templateInstaller.CreateProjectFromTemplate(projectPath, templatePath);

				return ProjectRootElement.Open(projectPath);
			}
			else return _projectParser.CreateProject(outputType, language, projectPath, projectName);
		}

	}
}