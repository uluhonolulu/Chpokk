using System.Collections.Generic;
using System.IO;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Remotes.DownloadZip;
using ChpokkWeb.Features.Storage;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryEndpoint {
		[NotNull]
		private readonly RepositoryCache _repositoryCache;
		[NotNull]
		private readonly RepositoryManager _manager;
		private Restore _restore;


		[UrlPattern("Repository/{RepositoryName}")]
		public RepositoryModel Get(RepositoryInputModel input) {
			// let's add it to the cache first
			var info = _manager.GetRepositoryInfo(input.RepositoryName);
			_repositoryCache[info.Path] = info;
			return new RepositoryModel() { RepositoryName = input.RepositoryName };
		}

		public RepositoryEndpoint([NotNull]RepositoryManager manager, [NotNull]RepositoryCache repositoryCache, Restore restore) {
			_manager = manager;
			_repositoryCache = repositoryCache;
			_restore = restore;
		}

		public RepositoryListModel GetRepositoryList([NotNull]RepositoryListInputModel model) {
			_restore.RestoreFilesForCurrentUser();
			return new RepositoryListModel {RepositoryNames = _manager.GetRepositoryNames()};
		}
	}
}
