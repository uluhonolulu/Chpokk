using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Core.Ajax;

namespace ChpokkWeb.Features.ProjectManagement.CreateEmptySolution {
	public class CreateEmptySolutionEndpoint {
		private readonly SolutionFileLoader _solutionFileLoader;
		private readonly RepositoryManager _repositoryManager;
		public CreateEmptySolutionEndpoint(SolutionFileLoader solutionFileLoader, RepositoryManager repositoryManager) {
			_solutionFileLoader = solutionFileLoader;
			_repositoryManager = repositoryManager;
		}

		public AjaxContinuation DoIt(CreateEmptySolutionInputModel model) {
			var repositoryName = model.RepositoryName;
			var solutionPath = _repositoryManager.NewGetAbsolutePathFor(repositoryName, repositoryName + ".sln");
			_solutionFileLoader.CreateEmptySolution(solutionPath);
			return new AjaxContinuation(){Success = true, ShouldRefresh = true};
		}
	}

	public class CreateEmptySolutionInputModel : BaseRepositoryInputModel { }
}