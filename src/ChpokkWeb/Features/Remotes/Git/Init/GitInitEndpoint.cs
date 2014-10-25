using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Core.Ajax;

namespace ChpokkWeb.Features.Remotes.Git.Init {
	public class GitInitEndpoint {
		private readonly RepositoryManager _repositoryManager;
		private readonly GitInitializer _initializer;
		public GitInitEndpoint(RepositoryManager repositoryManager, GitInitializer initializer) {
			_repositoryManager = repositoryManager;
			_initializer = initializer;
		}

		public AjaxContinuation DoIt(GitInitInputModel model) {
			var repositoryRoot = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName);
			_initializer.Init(repositoryRoot);
			return AjaxContinuation.Successful();
		}
	}

	public class GitInitInputModel: BaseRepositoryInputModel {}
}