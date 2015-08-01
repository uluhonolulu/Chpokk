using System.Collections.Generic;
using System.IO;
using ChpokkWeb.Features.Editor.Menu;
using ChpokkWeb.Features.Remotes.Git.Init;
using ChpokkWeb.Features.Remotes.Git.Remotes;
using ChpokkWeb.Infrastructure;
using ChpokkWeb.Infrastructure.Menu;
using FubuCore;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.Remotes.Git.Push {
	public class PushPolicy : IRetrievePolicy, IMenuItemSource, IEditorMenuPolicy {
		private readonly RemoteInfoProvider _remoteInfoProvider;
		private readonly GitInitializer _gitInitializer;

		public PushPolicy(RemoteInfoProvider remoteInfoProvider, GitInitializer gitInitializer) {
			_remoteInfoProvider = remoteInfoProvider;
			_gitInitializer = gitInitializer;
		}

		public bool Matches(string repositoryRoot) {
			var gitInitialized = _gitInitializer.GitRepositoryExistsIn(repositoryRoot);
			if (gitInitialized) {
				return _remoteInfoProvider.GetDefaultRemote(repositoryRoot).IsNotEmpty();
			}
			else
				return false;
		}

		public IEnumerable<MenuItem> GetMenuItems(string repositoryRoot) {
			var defaultRemote = _remoteInfoProvider.GetDefaultRemote(repositoryRoot);
			yield return new MenuItem() { Caption = "Publish to " + defaultRemote, Id = "push"};
		}


		public MenuItem GetMenuItem(string repositoryRoot) {
			var defaultRemote = _remoteInfoProvider.GetDefaultRemote(repositoryRoot);
			return new MenuItem(){Caption = "Git push to " + defaultRemote, Id = "pusher"};
		}
	}
}