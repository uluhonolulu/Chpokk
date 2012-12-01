using System;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Editor.SaveCommit {
	public class SaveCommitController {
		private readonly FileSystem _fileSystem;
		private RepositoryManager _manager;
		public SaveCommitController(FileSystem fileSystem, RepositoryManager manager) {
			_fileSystem = fileSystem;
			_manager = manager;
		}

		public void Save(SaveCommitModel saveCommitModel) {
			var filePath = _manager.GetPhysicalFilePath(saveCommitModel);
			_fileSystem.WriteStringToFile(filePath, saveCommitModel.Content);
			if (saveCommitModel.DoCommit) {
				var repositoryInfo = _manager.GetRepositoryInfo(saveCommitModel.RepositoryName);
				var repositoryPath = saveCommitModel.PhysicalApplicationPath.AppendPath(repositoryInfo.Path);
				using (var repo = new Repository(repositoryPath)) {
					repo.Commit(saveCommitModel.CommitMessage);
				}
			}
		}
	}
}