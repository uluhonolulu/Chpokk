using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;

namespace ChpokkWeb.Features.ProjectManagement.References.Files {
	public class UploadAssemblyEndpoint {
		private RepositoryManager _repositoryManager;
		public UploadAssemblyEndpoint(RepositoryManager repositoryManager) {
			_repositoryManager = repositoryManager;
		}

		public AjaxContinuation DoIt(UploadAssemblyInputModel model) {
			if (model.Assembly == null)
				return AjaxContinuation.Successful();
			var folderPath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, "lib");
			Directory.CreateDirectory(folderPath);
			var path = folderPath.AppendPath(model.Assembly.FileName);
			model.Assembly.SaveAs(path);
			return AjaxContinuation.Successful();
		}
	}

	public class UploadAssemblyInputModel: BaseRepositoryInputModel {
		public HttpPostedFileBase Assembly { get; set; }
	}
}