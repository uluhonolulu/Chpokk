using System;
using System.Collections.Generic;
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
		private readonly IEnumerable<ICommitter> _committers;
		public SaveCommitEndpoint(RepositoryManager manager, ISecurityContext securityContext, Savior savior, IEnumerable<ICommitter> committers) {
			_manager = manager;
			_securityContext = securityContext;
			_savior = savior;
			_committers = committers;
		}

		public void SaveCommit(SaveCommitInputModel model) {
			_savior.SaveFile(model);
			var filePath = _manager.NewGetAbsolutePathFor(model.RepositoryName, model.PathRelativeToRepositoryRoot);
			foreach (var committer in _committers) {
				if (committer.Matches(model.RepositoryName)) {
					committer.Commit(filePath, model.CommitMessage);
				}
			}
			//Commit(model);
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