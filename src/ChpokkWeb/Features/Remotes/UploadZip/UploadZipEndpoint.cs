using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure.SimpleZip;

namespace ChpokkWeb.Features.Remotes.UploadZip {
	public class UploadZipEndpoint {
		private readonly Zipper _zipper;
		private readonly RepositoryManager _repositoryManager;
		public UploadZipEndpoint(RepositoryManager repositoryManager, Zipper zipper) {
			_repositoryManager = repositoryManager;
			_zipper = zipper;
		}

		public string Upload(UploadZipInputModel model) {
			var repositoryPath = _repositoryManager.GetAbsolutePathFor("repo", model.PhysicalApplicationPath);
			_zipper.UnzipStream(repositoryPath, model.ZippedRepository.InputStream);
			return "OK";
		}

	}
}