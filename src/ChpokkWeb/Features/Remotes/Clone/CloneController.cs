using System.IO;
using ChpokkWeb.Features.Exploring;
using FubuMVC.Core;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;
using LibGit2Sharp;
using ChpokkWeb.Infrastructure;
using RepositoryInputModel = ChpokkWeb.Features.RepositoryManagement.RepositoryInputModel;

namespace ChpokkWeb.Features.Remotes.Clone {
	public class CloneController {

		private IUrlRegistry _registry;
		private RepositoryManager _repositoryManager;
		public CloneController(IUrlRegistry registry, RepositoryManager repositoryManager) {
			_registry = registry;
			_repositoryManager = repositoryManager;
		}

		[JsonEndpoint]
		public AjaxContinuation CloneRepository(CloneInputModel model) {
			var repoUrl = model.RepoUrl;
			var repositoryInfo = _repositoryManager.GetClonedRepositoryInfo(repoUrl);
			var repositoryPath = Path.Combine(model.PhysicalApplicationPath, repositoryInfo.Path);
			CloneGitRepository(repoUrl, repositoryPath);
			var projectUrl = _registry.UrlFor(new RepositoryInputModel() { Name = repositoryInfo.Name });
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}

		private static void CloneGitRepository(string repoUrl, string repositoryPath) {
			var repository = Repository.Clone(repoUrl, repositoryPath);
			repository.Dispose();
		}

		public string TestCloning(AppPathAwareInputModel model) {
			var repoUrl = "git://github.com/uluhonolulu/Chpokk-SampleSol.git";
			var repositoryPath = Path.Combine(model.PhysicalApplicationPath, @"UserFiles\Chpokk-SampleSol");
			var repository = Repository.Clone(repoUrl, repositoryPath);
			var master = repository.Branches["master"];
			repository.Checkout(master);
			repository.Dispose();
			return "success!";
		} 
	}

	public class CloneInputModel : IDontNeedActionsModel {
		public string RepoUrl { get; set; }
		public string PhysicalApplicationPath { get; set; }
	}

	public class AppPathAwareInputModel {
		public string PhysicalApplicationPath { get; set; }		
	}
}