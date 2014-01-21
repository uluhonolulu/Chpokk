using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Core;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Storage {
	public class Restore {
		private readonly Downloader _downloader;
		private readonly RepositoryManager _repositoryManager;

		public Restore(Downloader downloader, ApplicationSettings settings, RepositoryManager repositoryManager) {
			_downloader = downloader;
			_repositoryManager = repositoryManager;
		}

		protected string AppRoot { get; set; }

		public void RestoreAll() {
			if (!_repositoryManager.RepositoriesExist())
				_downloader.DownloadAllFiles(AppRoot);
		}

		public void RestoreFilesForCurrentUser() {
			if (_repositoryManager.ShouldRestore()) {
				_repositoryManager.RestoreFilesForCurrentUser();
			}
		}



	}
}