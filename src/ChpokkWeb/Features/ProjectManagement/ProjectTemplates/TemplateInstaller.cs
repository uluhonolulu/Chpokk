using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.ProjectTemplates {
	public class TemplateInstaller {
		private const string TEMPLATE_ROOT = @"SystemFiles\Templates\ProjectTemplates\";
		private readonly IAppRootProvider _rootProvider;
		private readonly FileSystem _fileSystem;
		private readonly TemplateTransformer _templateTransformer;
		public TemplateInstaller(FileSystem fileSystem, TemplateTransformer templateTransformer, IAppRootProvider rootProvider) {
			_fileSystem = fileSystem;
			_templateTransformer = templateTransformer;
			_rootProvider = rootProvider;
		}

		public void CreateProjectFromTemplate(string projectPath, string templateRelativePath) {
			var projectFolder = projectPath.ParentDirectory();
			var projectName = Path.GetFileNameWithoutExtension(projectPath);
			var templatePath = this.TemplateFolder.AppendPath(templateRelativePath);
			var projectTemplateFolder = templatePath.ParentDirectory();
			var replacements = new Dictionary<string, string>()
				{
					{"$safeprojectname$", projectName},
					{"$projectname$", projectName},
					{"$targetframeworkversion$", "4.5"},
					{"$registeredorganization$", "GeekSoft"},
					{"$year$", DateTime.Today.Year.ToString()},
					{"$guid1$", Guid.NewGuid().ToString()}
				};
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

		public IEnumerable<Template> GetTemplates() {
			var templateFiles = _fileSystem.FindFiles(TemplateFolder, new FileSet() { Include = "*.vstemplate", DeepSearch = true });
			return from file in templateFiles select Template.LoadTemplate(file);
		}

		public IEnumerable<ProjectTemplateData> GetTemplateData() {
			var templates = this.GetTemplates();
			return from template in templates
				   select new ProjectTemplateData() {
					   Name = template.Name,
					   Path = template.Path.PathRelativeTo(TemplateFolder),
					   RequiredFrameworkVersion = template.RequiredFrameworkVersion
				   };
			
		}

		private string TemplateFolder {
			get { return _rootProvider.AppRoot.AppendPath(TEMPLATE_ROOT); }
		}
	}
}