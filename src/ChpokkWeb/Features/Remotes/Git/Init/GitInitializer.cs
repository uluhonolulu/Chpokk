using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Git.Init {
	public class GitInitializer {
		public void Init(string repositoryRoot) {
			Repository.Init(repositoryRoot);
		}
	}
}