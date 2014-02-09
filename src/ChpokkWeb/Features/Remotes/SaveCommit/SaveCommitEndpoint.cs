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
			var repositoryPath = _manager.NewGetAbsolutePathFor(model.RepositoryName);
			foreach (var committer in _committers) {
				if (committer.Matches(repositoryPath)) {
					committer.Commit(filePath, model.CommitMessage, repositoryPath);
				}
			}
		}

	}
}