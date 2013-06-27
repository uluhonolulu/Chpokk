using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryEndpoint {
		[NotNull]
		private readonly RepositoryCache _repositoryCache;

		[UrlPattern("Repository/{Name}")]
		public RepositoryModel Get(RepositoryInputModel input) {
			// let's add it to the cache first
			var info = _manager.GetRepositoryInfo(input.Name);
			_repositoryCache[info.Path] = info;
			return new RepositoryModel(){Name = input.Name};
		}
		[NotNull]
		private readonly RepositoryManager _manager;
		public RepositoryEndpoint([NotNull]RepositoryManager manager, [NotNull]RepositoryCache repositoryCache) {
			_manager = manager;
			_repositoryCache = repositoryCache;
		}

		public RepositoryListModel GetRepositoryList([NotNull]RepositoryListInputModel model) {
			return new RepositoryListModel {RepositoryNames = _manager.GetRepositoryNames(model.PhysicalApplicationPath)};
		}
	}
}
