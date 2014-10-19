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
		private IAppRootProvider _appRootProvider;
		public ProjectCreator(ProjectParser projectParser, IFileSystem fileSystem, IAppRootProvider appRootProvider) {
			_projectParser = projectParser;
			_fileSystem = fileSystem;
			_appRootProvider = appRootProvider;
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
			var templateFileName = templateFilePath.PathRelativeTo(projectTemplateFolder);
			targetPath = targetPath?? projectFolder.AppendPath(templateFileName);
			var templateFileContent =
				_fileSystem.ReadStringFromFile(templateFilePath)
				           .Replace("$safeprojectname$", projectName)
				           .Replace("$targetframeworkversion$", "4.5")
				           .Replace("$guid1$", Guid.NewGuid().ToString());
			_fileSystem.WriteStringToFile(targetPath, templateFileContent);
		}
	}
}