using System.IO;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Ajax;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.AddProject {
	public class AddProjectEndpoint {
		private readonly ProjectParser _projectParser;
		private readonly RepositoryManager _repositoryManager;
		private readonly PackageInstaller _packageInstaller;
		private readonly SignalRLogger _logger;
		public AddProjectEndpoint(ProjectParser projectParser, RepositoryManager repositoryManager, PackageInstaller packageInstaller, SignalRLogger logger) {
			_projectParser = projectParser;
			_repositoryManager = repositoryManager;
			_packageInstaller = packageInstaller;
			_logger = logger;
		}

		public AjaxContinuation DoIt(AddProjectInputModel inputModel) {
			_logger.ConnectionId = inputModel.ConnectionId; 
			var solutionPath = _repositoryManager.NewGetAbsolutePathFor(inputModel.RepositoryName, inputModel.SolutionPath);
			_logger.WriteLine("Adding the project to solution {0}", inputModel.SolutionPath);
			_projectParser.AddProjectToSolution(inputModel.ProjectName, solutionPath, inputModel.Language);
			//create a project
			var projectFileName = inputModel.ProjectName + inputModel.Language.GetProjectExtension();
			var relativeProjectPath = Path.Combine(inputModel.SolutionPath.ParentDirectory(), inputModel.ProjectName, projectFileName);
			var projectPath = _repositoryManager.NewGetAbsolutePathFor(inputModel.RepositoryName, relativeProjectPath);
			var rootElement = _projectParser.CreateProject(inputModel.OutputType, inputModel.Language, projectPath, inputModel.ProjectName);

			//add references
			if (inputModel.References != null)
				foreach (var reference in inputModel.References) {
					_logger.WriteLine("Adding a reference to {0}", reference);
					_projectParser.AddReference(rootElement, reference);
				}
			if (inputModel.Projects != null)
				foreach (var relativeReferencedPath in inputModel.Projects) {
					_logger.WriteLine("Adding a reference to {0}", relativeReferencedPath);
					var referencedPath = solutionPath.ParentDirectory().AppendPath(relativeReferencedPath);
					_projectParser.AddProjectReference(rootElement, referencedPath);
				}

			//install packages
			var targetFolder = _repositoryManager.NewGetAbsolutePathFor(inputModel.RepositoryName).AppendPath("packages");
			if (inputModel.Packages != null) {
				foreach (var packageId in inputModel.Packages) {
					if (packageId.IsNotEmpty()) {
						//new Thread(() => {_packageInstaller.InstallPackage(packageId, projectPath, targetFolder);}).Start();
						_packageInstaller.InstallPackage(packageId, projectPath, targetFolder);
					}
				}
			}
			rootElement.Save();

			_logger.WriteLine("Project created");
			return new AjaxContinuation {ShouldRefresh = true};


		}
	}
}