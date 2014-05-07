using System.IO;
using System.Linq;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Construction;

namespace ChpokkWeb.Features.Exploring.Rename {
	public class RenameEndpoint {
		private readonly IFileSystem _fileSystem;
		private readonly RepositoryManager _repositoryManager;
		public RenameEndpoint(IFileSystem fileSystem, RepositoryManager repositoryManager) {
			_fileSystem = fileSystem;
			_repositoryManager = repositoryManager;
		}

		public AjaxContinuation DoIt(RenameInputModel model) {
			RenameFile(model);
			if (model.ItemType == "Project") {
				RenameProjectInSolution(model);
			}
			if (model.ItemType == "Item") {
				RenameItemInProject(model);
			}
			return AjaxContinuation.Successful();
		}

		private void RenameItemInProject(RenameInputModel model) {
			var projectFilePath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.ProjectPath);
			var project = ProjectRootElement.Open(projectFilePath);
			var pathRelativeToProjectFolder = model.PathRelativeToRepositoryRoot.PathRelativeTo(model.ProjectPath.ParentDirectory());
			var projectItem = project.Items.First(element => element.Include == pathRelativeToProjectFolder);
			var newPathRelativeToProjectFolder = pathRelativeToProjectFolder.ParentDirectory().AppendPath(model.NewFileName);
			projectItem.Include = newPathRelativeToProjectFolder;
			project.Save();			
		}

		private void RenameProjectInSolution(RenameInputModel model) {
			var solutionPath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.SolutionPath);
			var solutionContent = _fileSystem.ReadStringFromFile(solutionPath);
			var projectPathRelativeToSolution = model.PathRelativeToRepositoryRoot.PathRelativeTo(model.SolutionPath.ParentDirectory());
			var newProjectRelativePath = projectPathRelativeToSolution.ParentDirectory().AppendPath(model.NewFileName);
			var projectName = Path.GetFileNameWithoutExtension(model.PathRelativeToRepositoryRoot);
			var newProjectName = Path.GetFileNameWithoutExtension(model.NewFileName);
			solutionContent = solutionContent.Replace(projectPathRelativeToSolution, newProjectRelativePath).Replace("\"" + projectName + "\"", "\"" + newProjectName + "\"");
			_fileSystem.WriteStringToFile(solutionPath, solutionContent);			
		}

		private void RenameFile(RenameInputModel model) {
			var oldFilePath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.PathRelativeToRepositoryRoot);
			var fileFolder = oldFilePath.ParentDirectory();
			var newFilePath = fileFolder.AppendPath(model.NewFileName);
			_fileSystem.MoveFile(oldFilePath, newFilePath);
		}
	}
}