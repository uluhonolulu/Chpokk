using System.Collections.Generic;
using System.IO;
using ChpokkWeb.Features.CustomerDevelopment;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Remotes.DownloadZip;
using ChpokkWeb.Features.Storage;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryEndpoint {

		[NotNull]
		private readonly RepositoryManager _manager;
		private readonly Restore _restore;
		private ActivityTracker _tracker;

		[UrlPattern("Repository/{RepositoryName}")]
		public RepositoryModel Get(RepositoryInputModel input) {
			return new RepositoryModel() { RepositoryName = input.RepositoryName };
		}

		public RepositoryEndpoint([NotNull]RepositoryManager manager, Restore restore, ActivityTracker tracker) {
			_manager = manager;
			_restore = restore;
			_tracker = tracker;
		}

		public RepositoryListModel GetRepositoryList([NotNull]RepositoryListInputModel model) {
			_tracker.Record("Restoring");
			_restore.RestoreFilesForCurrentUser();
			_tracker.Record("Restored");
			return new RepositoryListModel {RepositoryNames = _manager.GetRepositoryNames()};
		}
	}
}
