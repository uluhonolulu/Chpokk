using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using ICSharpCode.NRefactory;
using Microsoft.Build.Construction;

namespace ChpokkWeb.Features.ProjectManagement.AddProject {
	public class ProjectCreator {
		private readonly IFileSystem _fileSystem;
		private ProjectParser _projectParser;
		public ProjectCreator(ProjectParser projectParser, IFileSystem fileSystem) {
			_projectParser = projectParser;
			_fileSystem = fileSystem;
		}

		public ProjectRootElement CreateProject(string outputType, string projectName, string projectPath,
											 SupportedLanguage language) {
			if (outputType == "Web") {
				// from template
				var projectTemplatePath =
					@"D:\Projects\Chpokk\src\ChpokkWeb\SystemFiles\Templates\ProjectTemplates\CSharp\Web\EmptyWebApplicationProject40\WebApplication.csproj";
				var projectSource = _fileSystem.ReadStringFromFile(projectTemplatePath);
				projectSource =
					projectSource.Replace("$safeprojectname$", projectName)
								 .Replace("$targetframeworkversion$", "4.5")
								 .Replace("$guid1$", Guid.NewGuid().ToString());
				_fileSystem.WriteStringToFile(projectPath, projectSource);
				var webConfigTemplatePath =
					@"D:\Projects\Chpokk\src\ChpokkWeb\SystemFiles\Templates\ProjectTemplates\CSharp\Web\EmptyWebApplicationProject40\Web.config";
				var webConfigSource = _fileSystem.ReadStringFromFile(webConfigTemplatePath)
												 .Replace("$targetframeworkversion$", "4.5");
				var webConfigPath = projectPath.ParentDirectory().AppendPath("Web.config");
				_fileSystem.WriteStringToFile(webConfigPath, webConfigSource);
				return ProjectRootElement.Open(projectPath);
			}
			else return _projectParser.CreateProject(outputType, language, projectPath, projectName);
		}
	}
}