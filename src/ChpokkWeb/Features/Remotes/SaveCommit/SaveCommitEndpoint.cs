using System;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Files;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Security;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.SaveCommit {
	public class SaveCommitEndpoint {
		private readonly RepositoryManager _manager;
		private readonly ISecurityContext _securityContext;
		private readonly Savior _savior;
		public SaveCommitEndpoint(RepositoryManager manager, ISecurityContext securityContext, Savior savior) {
			_manager = manager;
			_securityContext = securityContext;
			_savior = savior;
		}

		public void SaveCommit(SaveCommitInputModel model) {
			_savior.SaveFile(model);
			Commit(model);
		}

		private void Commit(SaveCommitInputModel model) {
			var repositoryInfo = _manager.GetRepositoryInfo(model.RepositoryName);
			var repositoryPath = model.PhysicalApplicationPath.AppendPath(repositoryInfo.Path);
			var userName = _securityContext.CurrentIdentity.Name;
			var author = new Signature(userName, userName, DateTimeOffset.Now);
			var filePath = _manager.GetPhysicalFilePath(model);
			using (var repo = new Repository(repositoryPath)) {
				repo.Index.Stage(filePath);
				repo.Commit(model.CommitMessage, author, author);
			}
		}
	}
}