using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Evaluation;

namespace ChpokkWeb.Features.ProjectManagement.AddItem {
	public class AddItemEndpoint {
		private RepositoryManager _repositoryManager;
		public AddItemEndpoint(RepositoryManager repositoryManager) {
			_repositoryManager = repositoryManager;
		}

		public AjaxContinuation DoIt(AddItemInputModel model) {
			var repositoryInfo = _repositoryManager.GetRepositoryInfo(model.RepositoryName);
			var projectFilePath = FileSystem.Combine(model.PhysicalApplicationPath, repositoryInfo.Path,
			                              model.ProjectPathRelativeToRepositoryRoot);
			var fileName =
				model.PathRelativeToRepositoryRoot.PathRelativeTo(model.ProjectPathRelativeToRepositoryRoot.ParentDirectory());
			var project = new Project(projectFilePath);
			project.AddItem("Compile", fileName);
			project.Save();
			return AjaxContinuation.Successful();
		}
	}

	public class AddItemInputModel: BaseFileInputModel {
		public string ProjectPathRelativeToRepositoryRoot { get; set; }
	}
}