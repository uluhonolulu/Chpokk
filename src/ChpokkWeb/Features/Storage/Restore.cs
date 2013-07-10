using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Storage {
	public class Restore {
		private readonly Downloader _downloader;
		private readonly FileSystem _fileSystem;
		public Restore(Downloader downloader, ApplicationSettings settings, FileSystem fileSystem) {
			_downloader = downloader;
			_fileSystem = fileSystem;
			AppRoot = settings.GetApplicationFolder();
		}

		protected string AppRoot { get; set; }

		public string UserFilesDir { get { return FileSystem.Combine(AppRoot, RepositoryManager.COMMON_REPOSITORY_FOLDER); } }

		public void RestoreAll() {
			if(!Restored)
				_downloader.DownloadAllFiles(UserFilesDir);
		}

		public bool Restored {
			get { return ChildDirectoriesExist(UserFilesDir); }
		}

		private bool ChildDirectoriesExist(string parent) {
			return _fileSystem.ChildDirectoriesFor(parent).Any();
		}
	}
}