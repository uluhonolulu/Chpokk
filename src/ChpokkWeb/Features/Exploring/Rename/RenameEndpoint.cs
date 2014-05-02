using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;

namespace ChpokkWeb.Features.Exploring.Rename {
	public class RenameEndpoint {
		private readonly IFileSystem _fileSystem;
		private readonly RepositoryManager _repositoryManager;
		public RenameEndpoint(IFileSystem fileSystem, RepositoryManager repositoryManager) {
			_fileSystem = fileSystem;
			_repositoryManager = repositoryManager;
		}

		public AjaxContinuation DoIt(RenameInputModel model) {
			var oldFilePath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.PathRelativeToRepositoryRoot);
			var fileFolder = oldFilePath.ParentDirectory();
			var newFilePath = fileFolder.AppendPath(model.NewFileName);
			_fileSystem.MoveFile(oldFilePath, newFilePath);
			return AjaxContinuation.Successful();
		}
	}
}