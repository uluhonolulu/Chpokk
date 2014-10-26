using System;
using System.Linq;
using LibGit2Sharp;

namespace Chpokk.Tests.Git {
	public class RemoteListEndpoint {
		public RemoteListModel GetRemoteInfo(RemoteListInputModel model) {
			return GetRemoteInfo(string.Empty);
		}
		public RemoteListModel GetRemoteInfo(string repositoryRoot) {
			using (var repository = new Repository(repositoryRoot)) {
				var remotes = repository.Network.Remotes.ToArray(); //enumerate this before we're disposed
				return new RemoteListModel
					{
						Remotes = from remote in remotes select remote.Name,
						DefaultRemote = repository.Head.Remote.Name
					};
			}
		}
	}

	public class RemoteListInputModel {}
}