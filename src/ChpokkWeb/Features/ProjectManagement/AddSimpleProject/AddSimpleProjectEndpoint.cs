using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Project;

namespace ChpokkWeb.Features.ProjectManagement.AddSimpleProject {
	public class AddSimpleProjectEndpoint {
		private readonly IFileSystem _fileSystem;
		private readonly SolutionFileLoader _solutionFileLoader;
		private readonly RepositoryManager _repositoryManager;
		private readonly ProjectParser _projectParser;
		private readonly IUrlRegistry _registry;
		public AddSimpleProjectEndpoint(IFileSystem fileSystem, SolutionFileLoader solutionFileLoader, RepositoryManager repositoryManager, ProjectParser projectParser, IUrlRegistry registry) {
			_fileSystem = fileSystem;
			_solutionFileLoader = solutionFileLoader;
			_repositoryManager = repositoryManager;
			_projectParser = projectParser;
			_registry = registry;
		}

		public AjaxContinuation DoIt(AddSimpleProjectInputModel inputModel) {
			var solutionPath = _repositoryManager.GetAbsolutePathFor(inputModel.RepositoryName, inputModel.PhysicalApplicationPath, inputModel.RepositoryName + ".sln");
			_solutionFileLoader.CreateEmptySolution(_fileSystem, solutionPath);
			var projectGuid = inputModel.Language == SupportedLanguage.CSharp ? ProjectTypeGuids.CSharp : ProjectTypeGuids.VBNet;
			var projectFileExtension = inputModel.Language == SupportedLanguage.CSharp ? ".csproj" : ".vbproj";
			_projectParser.AddProjectToSolution(inputModel.RepositoryName, solutionPath, projectGuid, projectFileExtension);

			//create a project
			var projectPath = _repositoryManager.GetAbsolutePathFor(inputModel.RepositoryName, inputModel.PhysicalApplicationPath, Path.Combine(inputModel.RepositoryName, inputModel.RepositoryName + projectFileExtension));
			var rootElement = _projectParser.CreateProject(inputModel.OutputType, inputModel.Language);
			foreach (var reference in inputModel.References) {
				_projectParser.AddReference(rootElement, reference);
			}
			rootElement.Save(projectPath);

			//create Program.exe
			if (inputModel.OutputType == "Exe") {
				var filename = inputModel.Language == SupportedLanguage.CSharp? "Program.cs" : "Module1.vb" ;
				var fileContent = FileContent(filename);
				_projectParser.CreateItem(projectPath, filename, fileContent);
			}

			var projectUrl = _registry.UrlFor(new RepositoryInputModel { RepositoryName = inputModel.RepositoryName });
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}

		private static string FileContent(string filename) {
			var assembly = Assembly.GetExecutingAssembly();
			var reader = new StreamReader(assembly.GetManifestResourceStream("ChpokkWeb.App_GlobalResources.FileTemplates." + filename));
			return reader.ReadToEnd();
		}
	}
}