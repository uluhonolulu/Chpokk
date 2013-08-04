using System.Collections.Generic;
using System.IO;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Remotes.DownloadZip;
using ChpokkWeb.Features.Remotes.Push;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryEndpoint {
		[NotNull]
		private readonly RepositoryCache _repositoryCache;

		[UrlPattern("Repository/{RepositoryName}")]
		public RepositoryModel Get(RepositoryInputModel input) {
			// let's add it to the cache first
			var info = _manager.GetRepositoryInfo(input.RepositoryName);
			_repositoryCache[info.Path] = info;
			return new RepositoryModel() { RepositoryName = input.RepositoryName };
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
