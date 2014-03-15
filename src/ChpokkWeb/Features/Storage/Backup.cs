using System;
using System.Collections.Generic;
using System.IO;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Storage {
	public class Backup: IDisposable {
		private readonly Uploader _uploader;
		private readonly IAppRootProvider _rootProvider;
		private readonly IList<string> _userFolders; 
		public Backup(Uploader uploader, IAppRootProvider rootProvider) {
			_uploader = uploader;
			_rootProvider = rootProvider;
			_userFolders = new List<string>();
		}

		public void RegisterUserFolder(string path) {
			if (!_userFolders.Contains(path)) {
				_userFolders.Add(path);
			}
		}

		public void PublishRepository(RepositoryInfo repositoryInfo) {
			_uploader.UploadFolder(Path.Combine(_rootProvider.AppRoot, repositoryInfo.Path), _rootProvider.AppRoot);
		}

		public void Dispose() {
			foreach (var userFolder in _userFolders) {
				_uploader.UploadFolder(userFolder, _rootProvider.AppRoot);
			}
		}
	}
}