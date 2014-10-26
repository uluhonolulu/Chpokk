using System.Linq;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Git.Remotes {
	public class RemoteListEndpoint {
		private readonly RepositoryManager _repositoryManager;
		public RemoteListEndpoint(RepositoryManager repositoryManager) {
			_repositoryManager = repositoryManager;
		}

		public RemoteListModel GetRemoteInfo(RemoteListInputModel model) {
			var repositoryRoot = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName);
			return GetRemoteInfo(repositoryRoot);
		}

		//TODO: extact this to a separate class
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

	public class RemoteListInputModel:BaseRepositoryInputModel {}
}