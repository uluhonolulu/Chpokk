using System.IO;
using System.Linq;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Construction;
using ChpokkWeb.Infrastructure;

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

		public AjaxContinuation Move(MoveInputModel model) {
			if (model.ItemType != "Item") {
				throw new InvalidDataException("I can move only files");
			}
			MoveFile(model);
			MoveItemInProject(model);
			return AjaxContinuation.Successful();
		}

		private void MoveItemInProject(MoveInputModel model) {
			var projectFilePath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.ProjectPath);
			var pathRelativeToProjectFolder = model.PathRelativeToRepositoryRoot.PathRelativeTo(model.ProjectPath.ParentDirectory());
			var fileName = model.PathRelativeToRepositoryRoot.GetFileNameUniversal();
			var newPathRelativeToProjectFolder = model.NewFolder.AppendPath(fileName).PathRelativeTo(model.ProjectPath.ParentDirectory());
			ChangeItemPathInProject(projectFilePath, pathRelativeToProjectFolder, newPathRelativeToProjectFolder);
		}

		private void MoveFile(MoveInputModel model) {
			var oldFilePath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.PathRelativeToRepositoryRoot);
			var fileName = model.PathRelativeToRepositoryRoot.GetFileNameUniversal();
			var newFilePath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.NewFolder).AppendPath(fileName);
			_fileSystem.MoveFile(oldFilePath, newFilePath);
		}

		private void RenameItemInProject(RenameInputModel model) {
			var projectFilePath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.ProjectPath);
			var pathRelativeToProjectFolder = model.PathRelativeToRepositoryRoot.PathRelativeTo(model.ProjectPath.ParentDirectory());
			var newPathRelativeToProjectFolder = pathRelativeToProjectFolder.ParentDirectory().AppendPath(model.NewFileName);
			ChangeItemPathInProject(projectFilePath, pathRelativeToProjectFolder, newPathRelativeToProjectFolder);
		}

		private void ChangeItemPathInProject(string projectFilePath, string pathRelativeToProjectFolder,
		                                     string newPathRelativeToProjectFolder) {
			var project = ProjectRootElement.Open(projectFilePath);
			var projectItem = project.Items.First(element => element.Include == pathRelativeToProjectFolder);
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

	public class MoveInputModel : BaseFileInputModel {
		public string NewFolder { get; set; }
		public string ItemType { get; set; }
		public string SolutionPath { get; set; }
		public string ProjectPath { get; set; }
	}
}