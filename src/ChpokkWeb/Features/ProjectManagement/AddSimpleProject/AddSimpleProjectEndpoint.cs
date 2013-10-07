using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;

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

		public void DoIt(AddSimpleProjectInputModel addSimpleProjectInputModel) {
			var solutionPath = _repositoryManager.GetAbsolutePathFor(addSimpleProjectInputModel.Name, addSimpleProjectInputModel.AppRoot, addSimpleProjectInputModel.Name + ".sln");
			_solutionFileLoader.CreateEmptySolution(_fileSystem, solutionPath);
			_projectParser.AddProjectToSolution(addSimpleProjectInputModel.Name, solutionPath);

			//create a project
			var projectPath = _repositoryManager.GetAbsolutePathFor(addSimpleProjectInputModel.Name, addSimpleProjectInputModel.AppRoot, Path.Combine(addSimpleProjectInputModel.Name, addSimpleProjectInputModel.Name + ".csproj"));
			_projectParser.CreateProjectFile(addSimpleProjectInputModel.OutputType, projectPath);
		}
	}

	public class AddSimpleProjectInputModel {
		public string Name { get; set; }

		public string AppRoot { get; set; }

		public string OutputType { get; set; }
	}
}