using System;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using FubuMVC.Core.Security;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.SaveCommit {
	public class SaveCommitController {
		private readonly FileSystem _fileSystem;
		private readonly RepositoryManager _manager;
		private ISecurityContext _securityContext;
		public SaveCommitController(FileSystem fileSystem, RepositoryManager manager, ISecurityContext securityContext) {
			_fileSystem = fileSystem;
			_manager = manager;
			_securityContext = securityContext;
		}

		public void Save(SaveCommitModel saveCommitModel) {
			var filePath = _manager.GetPhysicalFilePath(saveCommitModel);
			_fileSystem.WriteStringToFile(filePath, saveCommitModel.Content);
			if (saveCommitModel.DoCommit) {
				var repositoryInfo = _manager.GetRepositoryInfo(saveCommitModel.RepositoryName);
				var repositoryPath = saveCommitModel.PhysicalApplicationPath.AppendPath(repositoryInfo.Path);
				var userName = _securityContext.CurrentIdentity.Name;
				var author = new Signature(userName, userName, DateTimeOffset.Now);
				using (var repo = new Repository(repositoryPath)) {
					repo.Index.Stage(filePath);
					repo.Commit(saveCommitModel.CommitMessage, author);
				}
			}
		}
	}
}