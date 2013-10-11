using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Project;

namespace ChpokkWeb.Features.ProjectManagement.AddSimpleProject {
	public class AddSimpleProjectEndpoint {
		private readonly IFileSystem _fileSystem;
		private readonly SolutionFileLoader _solutionFileLoader;
		private readonly RepositoryManager _repositoryManager;
		private readonly ProjectParser _projectParser;
		public AddSimpleProjectEndpoint(IFileSystem fileSystem, SolutionFileLoader solutionFileLoader, RepositoryManager repositoryManager, ProjectParser projectParser) {
			_fileSystem = fileSystem;
			_solutionFileLoader = solutionFileLoader;
			_repositoryManager = repositoryManager;
			_projectParser = projectParser;
		}

		public AjaxContinuation DoIt(AddSimpleProjectInputModel inputModel) {
			var solutionPath = _repositoryManager.GetAbsolutePathFor(inputModel.RepositoryName, inputModel.PhysicalApplicationPath, inputModel.RepositoryName + ".sln");
			_solutionFileLoader.CreateEmptySolution(_fileSystem, solutionPath);
			var projectGuid = inputModel.Language == SupportedLanguage.CSharp ? ProjectTypeGuids.CSharp : ProjectTypeGuids.VBNet;
			_projectParser.AddProjectToSolution(inputModel.RepositoryName, solutionPath, projectGuid);

			//create a project
			var projectPath = _repositoryManager.GetAbsolutePathFor(inputModel.RepositoryName, inputModel.PhysicalApplicationPath, Path.Combine(inputModel.RepositoryName, inputModel.RepositoryName + ".csproj"));
			_projectParser.CreateProjectFile(inputModel.OutputType, projectPath);

			//create Program.exe
			if (inputModel.OutputType == "EXE") {
				var filename = inputModel.Language == SupportedLanguage.CSharp? "Program.cs" : "Module1.vb" ;
				var fileContent = FileContent(filename);
				_projectParser.CreateItem(projectPath, filename, fileContent);
			}

			return AjaxContinuation.Successful();
		}

		private static string FileContent(string filename) {
			var assembly = Assembly.GetExecutingAssembly();
			var reader = new StreamReader(assembly.GetManifestResourceStream("ChpokkWeb.App_GlobalResources.FileTemplates." + filename));
			return reader.ReadToEnd();
		}
	}

	public class AddSimpleProjectInputModel : BaseRepositoryInputModel {
		public string OutputType { get; set; }
		public SupportedLanguage Language { get; set; }
	}
}