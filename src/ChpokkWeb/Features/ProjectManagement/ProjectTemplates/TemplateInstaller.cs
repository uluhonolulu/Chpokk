using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement {
	public class TemplateInstaller {
		private readonly FileSystem _fileSystem;
		private readonly TemplateTransformer _templateTransformer;
		public TemplateInstaller(FileSystem fileSystem, TemplateTransformer templateTransformer) {
			_fileSystem = fileSystem;
			_templateTransformer = templateTransformer;
		}

		public void CreateProjectFromTemplate(string projectPath, string templatePath) {
			var projectFolder = projectPath.ParentDirectory();
			var projectName = Path.GetFileNameWithoutExtension(projectPath);
			var projectTemplateFolder = templatePath.ParentDirectory();
			var replacements = new Dictionary<string, string>() { { "$safeprojectname$", projectName }, { "$targetframeworkversion$", "4.5" }, { "$guid1$", Guid.NewGuid().ToString() } };
			var template = Template.LoadTemplate(templatePath);
			var projectItems = template.GetProjectItems();
			foreach (var projectItem in projectItems) {
				var templateFileRelativePath = projectItem.FileName;	//relative to template folder
				var templateFileSourcePath = projectTemplateFolder.AppendPath(templateFileRelativePath);
				if (!File.Exists(templateFileSourcePath)) {//sometimes all files are in the root folder despite the folder structure in the template file; let's search by name in the template folder
					templateFileSourcePath = Directory.EnumerateFiles(projectTemplateFolder, Path.GetFileName(projectItem.FileName)).First();
				}
				var destinationRelativePath = projectItem.TargetFileName;
				var destinationPath = projectFolder.AppendPath(destinationRelativePath);
				var templateFileContent = _fileSystem.ReadStringFromFile(templateFileSourcePath);
				var processedContent = _templateTransformer.Evaluate(templateFileContent, replacements);
				_fileSystem.WriteStringToFile(destinationPath, processedContent);

			}

			var projectFileSourceRelativePath = template.ProjectFileName;
			var projectFileSourcePath = projectTemplateFolder.AppendPath(projectFileSourceRelativePath);
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFileSourcePath);
			var processedProjectContent = _templateTransformer.Evaluate(projectFileContent, replacements);
			_fileSystem.WriteStringToFile(projectPath, processedProjectContent);
		}
	}
}