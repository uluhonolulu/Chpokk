using System.IO;
using System.Web;
using ChpokkWeb.Features.Project;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Shared;
using Elmah;
using FubuMVC.Core;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;
using LibGit2Sharp;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Remotes {
	public class CloneController {

		private IUrlRegistry _registry;
		private RepositoryManager _repositoryManager;
		public CloneController(IUrlRegistry registry, RepositoryManager repositoryManager) {
			_registry = registry;
			_repositoryManager = repositoryManager;
		}

		[JsonEndpoint]
		public AjaxContinuation CloneRepo(CloneInputModel model) {
			var repositoryInfo = CloneRepository(model);
			_repositoryManager.Register(repositoryInfo);
			var projectUrl = _registry.UrlFor(new ProjectInputModel() { Name = repositoryInfo.Name });
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}

		private RepositoryInfo CloneRepository(CloneInputModel input) {
			var repositoryInfo = _repositoryManager.GetClonedRepositoryInfo(input.RepoUrl);
			var repositoryPath = Path.Combine(input.PhysicalApplicationPath, repositoryInfo.Path);
			var repository = Repository.Clone(input.RepoUrl, repositoryPath);
			var master = repository.Branches["master"];
			repository.Checkout(master);
			repository.Dispose();
			return repositoryInfo;
		}

		public string TestCloning(AppPathAwareInputModel model) {
			var repoUrl = "git://github.com/uluhonolulu/Chpokk-SampleSol.git";
			var repositoryPath = Path.Combine(model.PhysicalApplicationPath, @"UserFiles\Chpokk-SampleSol");
			Repository.Clone(repoUrl, repositoryPath);
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