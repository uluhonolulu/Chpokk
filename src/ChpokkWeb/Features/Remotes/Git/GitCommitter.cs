using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Remotes.SaveCommit;
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
	}
}