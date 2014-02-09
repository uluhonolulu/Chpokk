using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Remotes.SaveCommit;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Core.Security;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Git {
	public class GitCommitter: GitDetectionPolicy, ICommitter {
		private readonly RepositoryManager _manager;
		private readonly ISecurityContext _securityContext;
		public GitCommitter(RepositoryManager repositoryManager, RepositoryManager manager, ISecurityContext securityContext) : base(repositoryManager) {
			_manager = manager;
			_securityContext = securityContext;
		}

		public void Commit(string filePath, string commitMessage, string repositoryName) {
			var userName = _securityContext.CurrentIdentity.Name;
			var author = new Signature(userName, userName, DateTimeOffset.Now);
			var repositoryPath = _manager.NewGetAbsolutePathFor(repositoryName);
			using (var repo = new Repository(repositoryPath)) {
				repo.Index.Stage(filePath);
				repo.Commit(commitMessage, author, author);
			}
		}
	}
}