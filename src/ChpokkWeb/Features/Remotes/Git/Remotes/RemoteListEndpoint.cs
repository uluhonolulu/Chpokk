using System.Linq;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Git.Remotes {
	public class RemoteListEndpoint {
		private readonly RepositoryManager _repositoryManager;
		private readonly RemoteInfoProvider _remoteInfoProvider;
		public RemoteListEndpoint(RepositoryManager repositoryManager, RemoteInfoProvider remoteInfoProvider) {
			_repositoryManager = repositoryManager;
			_remoteInfoProvider = remoteInfoProvider;
		}

		public RemoteListModel GetRemoteInfo(RemoteListInputModel model) {
			var repositoryRoot = _repositoryManager.GetAbsoluteRepositoryPath(model.RepositoryName);
			return GetRemoteInfo(repositoryRoot);
		}

		public RemoteListModel GetRemoteInfo(string repositoryRoot) {
			return new RemoteListModel
				{
					Remotes = _remoteInfoProvider.GetRemoteNames(repositoryRoot),
					DefaultRemote = _remoteInfoProvider.GetDefaultRemote(repositoryRoot)
				};
		}
	}

	public class RemoteListInputModel:BaseRepositoryInputModel {}
}