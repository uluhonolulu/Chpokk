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
		public DeleteRepositoryEndpoint(RepositoryManager repositoryManager, UrlRegistry urlRegistry, IFileSystem fileSystem) {
			_repositoryManager = repositoryManager;
			_urlRegistry = urlRegistry;
			_fileSystem = fileSystem;
		}

		public AjaxContinuation DoIt(DeleteRepositoryInputModel model) {
			var path = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName);
			_fileSystem.DeleteDirectory(path);
			var url = _urlRegistry.UrlFor<MainDummyModel>();
			return new AjaxContinuation().NavigateTo(url);
		}
	}

	public class DeleteRepositoryInputModel: BaseRepositoryInputModel {}
}