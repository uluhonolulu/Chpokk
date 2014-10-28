using System.IO;
using ChpokkWeb.Features.Remotes.Git.Remotes;
using ChpokkWeb.Infrastructure;
using ChpokkWeb.Infrastructure.Menu;
using FubuCore;

namespace ChpokkWeb.Features.Remotes.Git.Push {
	public class PushPolicy: IRetrievePolicy, IMenuItemSource {
		private readonly RemoteInfoProvider _remoteInfoProvider;

		public PushPolicy(RemoteInfoProvider remoteInfoProvider) {
			_remoteInfoProvider = remoteInfoProvider;
		}

		public bool Matches(string repositoryRoot) {
			var path = FileSystem.Combine(repositoryRoot, ".git");
			var gitInitialized = Directory.Exists(path);
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