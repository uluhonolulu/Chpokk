using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using ChpokkWeb.Infrastructure.Menu;
using FubuCore;

namespace ChpokkWeb.Features.Remotes.Git.Init {
	public class GitInitPolicy: IRetrievePolicy, IMenuItemSource {
		private readonly GitInitializer _gitInitializer;
		public GitInitPolicy(GitInitializer gitInitializer) {
			_gitInitializer = gitInitializer;
		}

		public bool Matches(string repositoryRoot) {
			//match when we don't have a git repository
			return !_gitInitializer.GitRepositoryExistsIn(repositoryRoot);
		}

		public MenuItem GetMenuItem(string repositoryRoot) {
			return new MenuItem(){Caption = "Git init", Id = "gitinit"};
		}
	}
}