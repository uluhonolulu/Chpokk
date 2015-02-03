﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement {
	public class TemplateInstaller {
		private FileSystem _fileSystem;
		private TemplateTransformer _templateTransformer;
		public TemplateInstaller(FileSystem fileSystem, TemplateTransformer templateTransformer) {
			_fileSystem = fileSystem;
			_templateTransformer = templateTransformer;
		}

		public void CreateProjectFromTemplate(string projectPath, string templatePath) {
			var projectFolder = projectPath.ParentDirectory();
			var projectName = Path.GetFileNameWithoutExtension(projectPath);
			var projectTemplateFolder = templatePath.ParentDirectory();
			var replacements = new Dictionary<string, string>() { { "$safeprojectname$", projectName }, { "$targetframeworkversion$", "4.5" }, { "$guid1$", Guid.NewGuid().ToString() } };
			var template = new Template(templatePath);
			var projectItems = template.GetProjectItems();
			foreach (var projectItem in projectItems) {
				var templateFileRelativePath = projectItem.FileName;	//relative to template folder
				var templateFileSourcePath = projectTemplateFolder.AppendPath(templateFileRelativePath);
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