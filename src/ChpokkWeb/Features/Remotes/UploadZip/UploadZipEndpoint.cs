using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure.SimpleZip;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Remotes.UploadZip {
	public class UploadZipEndpoint {
		private readonly Zipper _zipper;
		private readonly RepositoryManager _repositoryManager;
		private readonly IUrlRegistry _registry;
		public UploadZipEndpoint(RepositoryManager repositoryManager, Zipper zipper, IUrlRegistry registry) {
			_repositoryManager = repositoryManager;
			_zipper = zipper;
			_registry = registry;
		}

		public AjaxContinuation Upload(UploadZipInputModel model) {
			var repositoryName = Path.GetFileNameWithoutExtension(model.ZippedRepository.FileName);
			var repositoryPath = _repositoryManager.GetAbsolutePathFor(repositoryName, model.PhysicalApplicationPath);
			_zipper.UnzipStream(repositoryPath, model.ZippedRepository.InputStream);
			var projectUrl = _registry.UrlFor(new RepositoryInputModel() { RepositoryName = repositoryName });
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}

	}
}