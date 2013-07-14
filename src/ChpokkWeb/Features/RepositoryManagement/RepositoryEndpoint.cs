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
			return new RepositoryModel() { RepositoryName = input.RepositoryName, RetrieveActions = GetRetrieveActions() };
		}

		public static MenuItem[] GetRetrieveActions() {
			var menuItems = new List<MenuItem>();
			menuItems.Add(new DownloadZipMenuItem());
			var path = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\ulu\Perka\.git";
			if (Directory.Exists(path)) {
				menuItems.Add(new PushMenuItem());
			}
			return menuItems.ToArray();
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
