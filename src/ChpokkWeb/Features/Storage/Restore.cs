using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ChpokkWeb.Features.CustomerDevelopment;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuCore;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Storage {
	public class Restore {
		private readonly Downloader _downloader;
		private readonly RepositoryManager _repositoryManager;
		private readonly ActivityTracker _activityTracker;
		private readonly IFileSystem _fileSystem;
		private IAppRootProvider _appRootProvider;

		public Restore(Downloader downloader, RepositoryManager repositoryManager, ActivityTracker activityTracker, IFileSystem fileSystem, IAppRootProvider appRootProvider) {
			_downloader = downloader;
			_repositoryManager = repositoryManager;
			_activityTracker = activityTracker;
			_fileSystem = fileSystem;
			_appRootProvider = appRootProvider;
		}


		public void RestoreAll() {
			if (!_repositoryManager.RepositoriesExist())
				_downloader.DownloadAllFiles(_appRootProvider.AppRoot);
		}

		public void RestoreFilesForCurrentUserIfNeeded() {
			_activityTracker.Record("Should we restore?");
			if (this.ShouldRestore()) {
				this.RestoreFilesForCurrentUser();
			}
		}

		public bool ShouldRestore() {
			return !Directory.Exists(_repositoryManager.GetUserFolder()) || !_fileSystem.HasChildFolders(_repositoryManager.GetUserFolder()); //user folder does not exist or is empty
		}

		public void RestoreFilesForCurrentUser() {
			//create folders so that we see the list of repositories -- the files will be downloaded while we are staring at that list
			_activityTracker.Record("Getting repository list");
			var repositoryNames = _repositoryManager.GetRepositoryNamesFromStorage();
			foreach (var repositoryName in repositoryNames) {
				_activityTracker.Record("Creating folder for " + repositoryName);
				var repositoryPath = _repositoryManager.GetAbsoluteRepositoryPath(repositoryName);
				Directory.CreateDirectory(repositoryPath);
				//download each repository asynchronously
				//let's not -- too many threads busy
				//Task.Run(() => _downloader.DownloadAllFiles(AppRoot, repositoryPath.PathRelativeTo(AppRoot), RecordDownloadedFile));

			}
			//downloading all files that are not there yet (other than repos)
			var relativeUserFolder = _repositoryManager.GetUserFolder().PathRelativeTo(_appRootProvider.AppRoot);
			new Thread(() => _downloader.DownloadAllFiles(_appRootProvider.AppRoot, relativeUserFolder, RecordDownloadedFile)).Start();
		}


		private void RecordDownloadedFile(string remotePath, string localPath) {
			_activityTracker.Record("Downloaded {0} to {1}".ToFormat(remotePath, localPath));
		}


	}

	public static class FileSystemExtensions {
		public static bool HasChildFolders(this IFileSystem fileSystem, string parentFolder) {
			return fileSystem.ChildDirectoriesFor(parentFolder).Any();
		}
	}
}