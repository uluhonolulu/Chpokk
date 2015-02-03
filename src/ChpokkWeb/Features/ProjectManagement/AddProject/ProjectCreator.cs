using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
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
		private TemplateInstaller _templateInstaller;
		public ProjectCreator(ProjectParser projectParser, IFileSystem fileSystem, IAppRootProvider appRootProvider, TemplateTransformer templateTransformer, TemplateInstaller templateInstaller) {
			_projectParser = projectParser;
			_fileSystem = fileSystem;
			_appRootProvider = appRootProvider;
			_templateTransformer = templateTransformer;
			_templateInstaller = templateInstaller;
		}

		public ProjectRootElement CreateProject(string outputType, string projectName, string projectPath,
											 SupportedLanguage language) {
			if (outputType == "Web") {
				// from template
				var projectTemplatePath =
					_appRootProvider.AppRoot.AppendPath(
						@"SystemFiles\Templates\ProjectTemplates\{0}\Web\Version2012\1033\EmptyWebApplicationProject40\EmptyWebApplicationProject40.vstemplate".ToFormat(language == SupportedLanguage.CSharp ? "CSharp" : "VisualBasic"));
				_templateInstaller.CreateProjectFromTemplate(projectPath, projectTemplatePath);

				return ProjectRootElement.Open(projectPath);
			}
			else return _projectParser.CreateProject(outputType, language, projectPath, projectName);
		}

		private void AddFileFromTemplate(string projectName, string templateFilePath, string projectTemplateFolder,
										 string projectFolder, string targetPath = null) {
			var templateFileName = templateFilePath.PathRelativeTo(projectTemplateFolder).TrimEnd('_'); //trim the last char so that we handle web.config_
			targetPath = targetPath?? projectFolder.AppendPath(templateFileName);
			var templateFileContent =
				_fileSystem.ReadStringFromFile(templateFilePath);
			var replacements = new Dictionary<string, string>() {{"$safeprojectname$", projectName}, {"$targetframeworkversion$", "4.5"}, {"$guid1$", Guid.NewGuid().ToString()}};
			var processedContent = _templateTransformer.Evaluate(templateFileContent, replacements);
			_fileSystem.WriteStringToFile(targetPath, processedContent);
		}
	}
}