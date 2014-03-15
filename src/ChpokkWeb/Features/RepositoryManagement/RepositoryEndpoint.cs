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
		private readonly RepositoryManager _manager;
		private Restore _restore;


		[UrlPattern("Repository/{RepositoryName}")]
		public RepositoryModel Get(RepositoryInputModel input) {
			return new RepositoryModel() { RepositoryName = input.RepositoryName };
		}

		public RepositoryEndpoint([NotNull]RepositoryManager manager, Restore restore) {
			_manager = manager;
			_restore = restore;
		}

		public RepositoryListModel GetRepositoryList([NotNull]RepositoryListInputModel model) {
			_restore.RestoreFilesForCurrentUser();
			return new RepositoryListModel {RepositoryNames = _manager.GetRepositoryNames()};
		}
	}
}
