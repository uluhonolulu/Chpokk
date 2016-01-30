using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.ProjectTemplates {
	public class TemplateInstaller {
		private const string TEMPLATE_ROOT = @"SystemFiles\Templates\ProjectTemplates\";
		private readonly IAppRootProvider _rootProvider;
		private readonly FileSystem _fileSystem;
		private readonly TemplateTransformer _templateTransformer;
		private PackageInstaller _packageInstaller;
		public TemplateInstaller(FileSystem fileSystem, TemplateTransformer templateTransformer, IAppRootProvider rootProvider, PackageInstaller packageInstaller) {
			_fileSystem = fileSystem;
			_templateTransformer = templateTransformer;
			_rootProvider = rootProvider;
			_packageInstaller = packageInstaller;
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
					{"$nugetpackagesfolder$", @"..\packages\"},
					{"$targetframeworkversion$", "4.5"},
					{"$clrversion$", "4.0"},
					{"$registeredorganization$", "GeekSoft"},
					{"$year$", DateTime.Today.Year.ToString()},
					{"$guid1$", Guid.NewGuid().ToString()},
					{"$guid2$", Guid.NewGuid().ToString()}
				};
			var template = Template.LoadTemplate(templatePath);
			foreach (var replacement in template.Replacements) {
				replacements[replacement.Key] = replacement.Value;
			}
			var projectItems = template.GetProjectItems();
			foreach (var projectItem in projectItems) {
				var templateFileRelativePath = projectItem.FileName;	//relative to template folder
				var templateFileSourcePath = projectTemplateFolder.AppendPath(templateFileRelativePath);
				if (!File.Exists(templateFileSourcePath)) {//sometimes all files are in the root folder despite the folder structure in the template file; let's search by name in the template folder
					templateFileSourcePath = Directory.EnumerateFiles(projectTemplateFolder, Path.GetFileName(projectItem.FileName)).FirstOrDefault();
					if (templateFileSourcePath.IsEmpty()) {
						throw new Exception("Couldn't find template file: " + projectItem.FileName);
					}
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

			//adding referenced packages
			var templatePackages = template.TemplatePackages;
			foreach (var package in templatePackages.Packages) {
				_packageInstaller.CopyPackageFromLocalFolder(templatePackages.PackageRepositoryPath, package.PackageId, package.Version, projectPath);
			}
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