using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore;
using FubuMVC.Core;

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

		public void RestoreAll() {
			if(!Restored) 
				_downloader.DownloadAllFiles(AppRoot);
		}

		public bool Restored {
			get { return ChildDirectoriesExist(AppRoot); }
		}

		private bool ChildDirectoriesExist(string parent) {
			return _fileSystem.ChildDirectoriesFor(parent).Any();
		}
	}
}