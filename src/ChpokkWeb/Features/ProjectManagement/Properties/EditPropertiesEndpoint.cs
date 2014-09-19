using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.AddProject;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Construction;

namespace ChpokkWeb.Features.ProjectManagement.Properties {
	public class EditPropertiesEndpoint : AddProjectBase {
		public EditPropertiesEndpoint(ProjectParser projectParser, RepositoryManager repositoryManager, PackageInstaller packageInstaller, SignalRLogger logger) : base(projectParser, repositoryManager, packageInstaller, logger) {}

		public AjaxContinuation Save(EditPropertiesInputModel inputModel) {
			_logger.ConnectionId = inputModel.ConnectionId;
			var repositoryPath = _repositoryManager.NewGetAbsolutePathFor(inputModel.RepositoryName);
			var solutionPath = _repositoryManager.NewGetAbsolutePathFor(inputModel.RepositoryName, inputModel.SolutionPath);
			var projectPath = _repositoryManager.NewGetAbsolutePathFor(inputModel.RepositoryName, inputModel.ProjectPath);
			var rootElement = ProjectRootElement.Open(projectPath); //TODO: load properly

			//add references
			_projectParser.ClearReferences(rootElement);
			//_packageInstaller.ClearPackages(repositoryPath);
			AddBclReferences(inputModel, rootElement);
			AddProjectReferences(inputModel, solutionPath, rootElement);
			AddPackages(inputModel, projectPath);
			AddFileReferences(inputModel, rootElement);
			rootElement.Save();

			_logger.WriteLine("Project saved");
			return new AjaxContinuation { ShouldRefresh = true };
		}
	}

	public class EditPropertiesInputModel: AddProjectInputModel {
		public string ProjectPath { get; set; }
	}
}