using System.IO;
using ChpokkWeb.Features.Remotes.Git.Init;
using ChpokkWeb.Features.Remotes.Git.Remotes;
using ChpokkWeb.Infrastructure;
using ChpokkWeb.Infrastructure.Menu;
using FubuCore;

namespace ChpokkWeb.Features.Remotes.Git.Push {
	public class PushPolicy: IRetrievePolicy, IMenuItemSource {
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

		public MenuItem GetMenuItem(string repositoryRoot) {
			var defaultRemote = _remoteInfoProvider.GetDefaultRemote(repositoryRoot);
			return new MenuItem(){Caption = "Git push to " + defaultRemote, Id = "pusher"};
		}
	}
}