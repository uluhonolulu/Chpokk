using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Remotes.SaveCommit;
using FubuCore;
using FubuMVC.Core.Security;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Git {
	public class GitCommitter: GitDetectionPolicy, ICommitter {
		private readonly ISecurityContext _securityContext;
		public GitCommitter(ISecurityContext securityContext) : base() {
			_securityContext = securityContext;
		}

		public void Commit(string filePath, string commitMessage, string repositoryPath) {
			var userName = _securityContext.CurrentIdentity.Name;
			var author = new Signature(userName, userName, DateTimeOffset.Now);
			using (var repo = new Repository(repositoryPath)) {
				repo.Index.Add(filePath);
				repo.Commit(commitMessage, author, author);
			}
		}

		public void Commit(IEnumerable<string> filePaths, string commitMessage, string repositoryPath) {
			var userName = _securityContext.CurrentIdentity.Name;
			var author = new Signature(userName, userName, DateTimeOffset.Now);
			using (var repo = new Repository(repositoryPath)) {
				filePaths.Each(filePath => repo.Index.Add(filePath));
				repo.Commit(commitMessage, author, author);
			}			
		}

		public void CommitAll(string commitMessage, string repositoryPath) {
			var userName = _securityContext.CurrentIdentity.Name;
			var author = new Signature(userName, userName, DateTimeOffset.Now);
			using (var repo = new Repository(repositoryPath)) {
				var repositoryStatus = repo.RetrieveStatus();
				if (repositoryStatus.IsDirty) {
					var entries = repositoryStatus.Untracked.Concat(repositoryStatus.Modified);// repositoryStatus.Added.Concat(repositoryStatus.Modified);
					var filePaths = from entry in entries select entry.FilePath;
					filePaths.Each(filePath => repo.Index.Add(filePath));
					repo.Commit(commitMessage, author, author);
				}
			}
		}
	}
}