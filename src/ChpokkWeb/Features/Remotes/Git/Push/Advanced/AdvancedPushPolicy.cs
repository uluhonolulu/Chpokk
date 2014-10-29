using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Remotes.Git.Init;
using ChpokkWeb.Infrastructure;
using ChpokkWeb.Infrastructure.Menu;

namespace ChpokkWeb.Features.Remotes.Git.Push.Advanced {
	public class AdvancedPushPolicy: IRetrievePolicy, IMenuItemSource {
		private readonly GitInitializer _gitInitializer;
		public AdvancedPushPolicy(GitInitializer gitInitializer) {
			_gitInitializer = gitInitializer;
		}

		public bool Matches(string repositoryRoot) {
			return _gitInitializer.GitRepositoryExistsIn(repositoryRoot);
		}

		public MenuItem GetMenuItem(string repositoryRoot) {
			return new MenuItem{Caption = "Git push to..", Id = "advancedPusher"};
		}
	}
}