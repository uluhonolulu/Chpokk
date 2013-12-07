using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Project;
using Microsoft.Build.Evaluation;

namespace ChpokkWeb.Features.ProjectManagement.AddSimpleProject {
	public class AddSimpleProjectEndpoint {
		private readonly IFileSystem _fileSystem;
		private readonly SolutionFileLoader _solutionFileLoader;
		private readonly RepositoryManager _repositoryManager;
		private readonly ProjectParser _projectParser;
		private readonly IUrlRegistry _registry;
		private PackageInstaller _packageInstaller;
		public AddSimpleProjectEndpoint(IFileSystem fileSystem, SolutionFileLoader solutionFileLoader, RepositoryManager repositoryManager, ProjectParser projectParser, IUrlRegistry registry, PackageInstaller packageInstaller) {
			_fileSystem = fileSystem;
			_solutionFileLoader = solutionFileLoader;
			_repositoryManager = repositoryManager;
			_projectParser = projectParser;
			_registry = registry;
			_packageInstaller = packageInstaller;
		}

		public AjaxContinuation DoIt(AddSimpleProjectInputModel inputModel) {
			var solutionPath = _repositoryManager.GetAbsolutePathFor(inputModel.RepositoryName, inputModel.PhysicalApplicationPath, inputModel.RepositoryName + ".sln");
			_solutionFileLoader.CreateEmptySolution(solutionPath);
			var projectGuid = inputModel.Language == SupportedLanguage.CSharp ? ProjectTypeGuids.CSharp : ProjectTypeGuids.VBNet;
			var projectFileExtension = inputModel.Language == SupportedLanguage.CSharp ? ".csproj" : ".vbproj";
			_projectParser.AddProjectToSolution(inputModel.RepositoryName, solutionPath, projectGuid, projectFileExtension);

			//create a project
			var projectPath = _repositoryManager.GetAbsolutePathFor(inputModel.RepositoryName, inputModel.PhysicalApplicationPath, Path.Combine(inputModel.RepositoryName, inputModel.RepositoryName + projectFileExtension));
			var rootElement = _projectParser.CreateProject(inputModel.OutputType, inputModel.Language);
			if (inputModel.References != null)
				foreach (var reference in inputModel.References) {
					_projectParser.AddReference(rootElement, reference);
				}
			rootElement.Save(projectPath);
			ProjectCollection.GlobalProjectCollection.UnloadProject(rootElement);

			//create Program.exe
			if (inputModel.OutputType == "Exe") {
				var filename = inputModel.Language == SupportedLanguage.CSharp? "Program.cs" : "Module1.vb" ;
				var fileContent = GetFileContent(filename);
				_projectParser.CreateItem(projectPath, filename, fileContent);
			}

			//install packages
			var targetFolder = _repositoryManager.GetAbsolutePathFor(inputModel.RepositoryName,
			                                                         inputModel.PhysicalApplicationPath).AppendPath("packages");
			if (inputModel.Packages != null) {
				foreach (var packageId in inputModel.Packages) {
					if (packageId.IsNotEmpty()) {
						//new Thread(() => {_packageInstaller.InstallPackage(packageId, projectPath, targetFolder);}).Start();
						_packageInstaller.InstallPackage(packageId, projectPath, targetFolder);
					}
				}
			}
			rootElement.Save(projectPath);

			var projectUrl = _registry.UrlFor(new RepositoryInputModel { RepositoryName = inputModel.RepositoryName });
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}

		private static string GetFileContent(string filename) {
			var assembly = Assembly.GetExecutingAssembly();
			var reader = new StreamReader(assembly.GetManifestResourceStream("ChpokkWeb.App_GlobalResources.FileTemplates." + filename));
			return reader.ReadToEnd();
		}
	}
}