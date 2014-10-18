using System.IO;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.AddSimpleProject;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Ajax;
using FubuCore;
using Microsoft.Build.Construction;

namespace ChpokkWeb.Features.ProjectManagement.AddProject {
	public class AddProjectEndpoint : AddProjectBase {
		public AddProjectEndpoint(ProjectParser projectParser, RepositoryManager repositoryManager, PackageInstaller packageInstaller, SignalRLogger logger, ProjectCreator projectCreator) : base(projectParser, repositoryManager, packageInstaller, logger, projectCreator) {}

		public AjaxContinuation DoIt(AddProjectInputModel inputModel) {
			_logger.ConnectionId = inputModel.ConnectionId; 
			var solutionPath = _repositoryManager.NewGetAbsolutePathFor(inputModel.RepositoryName, inputModel.SolutionPath);
			_logger.WriteLine("Adding the project to solution {0}", inputModel.SolutionPath);
			_projectParser.AddProjectToSolution(inputModel.ProjectName, solutionPath, inputModel.Language);
			//create a project
			var projectFileName = inputModel.ProjectName + inputModel.Language.GetProjectExtension();
			var relativeProjectPath = Path.Combine(inputModel.SolutionPath.ParentDirectory(), inputModel.ProjectName, projectFileName);
			var projectPath = _repositoryManager.NewGetAbsolutePathFor(inputModel.RepositoryName, relativeProjectPath);
			var rootElement = _projectCreator.CreateProject(inputModel.OutputType, inputModel.ProjectName, projectPath, inputModel.Language);

			//add references
			AddBclReferences(inputModel, rootElement);
			AddProjectReferences(inputModel, solutionPath, rootElement);
			AddPackages(inputModel, projectPath);
			rootElement.Save();

			_logger.WriteLine("Project created");
			return new AjaxContinuation {ShouldRefresh = true};


		}
	}
}