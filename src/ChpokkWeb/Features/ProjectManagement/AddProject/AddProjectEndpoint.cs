using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Ajax;
using ICSharpCode.SharpDevelop.Project;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.AddProject {
	public class AddProjectEndpoint {
		private readonly ProjectParser _projectParser;
		private readonly RepositoryManager _repositoryManager;
		public AddProjectEndpoint(ProjectParser projectParser, RepositoryManager repositoryManager) {
			_projectParser = projectParser;
			_repositoryManager = repositoryManager;
		}

		public AjaxContinuation DoIt(AddProjectInputModel inputModel) {
			var solutionPath = _repositoryManager.NewGetAbsolutePathFor(inputModel.RepositoryName, inputModel.SolutionPath);
			_projectParser.AddProjectToSolution(inputModel.ProjectName, solutionPath, inputModel.Language);
			//create a project
			var projectFileName = inputModel.ProjectName + inputModel.Language.GetProjectExtension();
			var relativeProjectPath = Path.Combine(inputModel.SolutionPath.ParentDirectory(), inputModel.ProjectName, projectFileName);
			var projectPath = _repositoryManager.NewGetAbsolutePathFor(inputModel.RepositoryName, relativeProjectPath);
			var rootElement = _projectParser.CreateProject(inputModel.OutputType, inputModel.Language, projectPath);

			return new AjaxContinuation(){ShouldRefresh = true};


		}
	}
}