using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.MainScreen;
using FubuCore;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.RepositoryManagement.DeleteRepository {
	public class DeleteRepositoryEndpoint {
		private readonly RepositoryManager _repositoryManager;
		private readonly UrlRegistry _urlRegistry;
		private readonly IFileSystem _fileSystem;
		private FubuCore.FileSystem _localFileSystem;
		public DeleteRepositoryEndpoint(RepositoryManager repositoryManager, UrlRegistry urlRegistry, IFileSystem fileSystem, FubuCore.FileSystem localFileSystem) {
			_repositoryManager = repositoryManager;
			_urlRegistry = urlRegistry;
			_fileSystem = fileSystem;
			_localFileSystem = localFileSystem;
		}

		public AjaxContinuation DoIt(DeleteRepositoryInputModel model) {
			var path = _repositoryManager.GetAbsoluteRepositoryPath(model.RepositoryName);
			_fileSystem.DeleteDirectory(path);
			var url = _urlRegistry.UrlFor<MainDummyModel>();
			return new AjaxContinuation().NavigateTo(url);
		}

		//delete all local repositories (for testing) 
		public AjaxContinuation DeleteAll() {
			var myRepos = _repositoryManager.GetRepositoryNames();
			foreach (var repo in myRepos) {
				var path = _repositoryManager.GetAbsoluteRepositoryPath(repo);
				_localFileSystem.DeleteDirectory(path);
				
			}
			return new AjaxContinuation() { NavigatePage = "/authentication" };
		}
	}

	public class DeleteRepositoryInputModel: BaseRepositoryInputModel {}
}