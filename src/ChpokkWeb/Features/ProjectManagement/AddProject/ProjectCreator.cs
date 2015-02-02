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
		private TemplateTransformer _templateTransformer;
		public ProjectCreator(ProjectParser projectParser, IFileSystem fileSystem, IAppRootProvider appRootProvider, TemplateTransformer templateTransformer) {
			_projectParser = projectParser;
			_fileSystem = fileSystem;
			_appRootProvider = appRootProvider;
			_templateTransformer = templateTransformer;
		}

		public ProjectRootElement CreateProject(string outputType, string projectName, string projectPath,
											 SupportedLanguage language) {
			if (outputType == "Web") {
				// from template
				var projectFolder = projectPath.ParentDirectory();
				var projectTemplateFolder =
					_appRootProvider.AppRoot.AppendPath(
						@"SystemFiles\Templates\ProjectTemplates\{0}\Web\EmptyWebApplicationProject40\".ToFormat(language == SupportedLanguage.CSharp ? "CSharp" : "VisualBasic"));
				var templateFiles = Directory.EnumerateFiles(projectTemplateFolder, "*.*", SearchOption.AllDirectories);
				var projectExtension = Path.GetExtension(projectPath);
				var projectTemplatePath = projectTemplateFolder.AppendPath("WebApplication" + projectExtension);
				foreach (var templateFilePath in templateFiles) {
					if (templateFilePath == projectTemplatePath) {
						AddFileFromTemplate(projectName, projectTemplatePath, projectTemplateFolder, projectFolder, projectPath);					
					}
					else {
						AddFileFromTemplate(projectName, templateFilePath, projectTemplateFolder, projectFolder);		
					}
				}

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