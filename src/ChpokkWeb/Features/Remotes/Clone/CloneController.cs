using System.IO;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Core;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;
using LibGit2Sharp;
using ChpokkWeb.Infrastructure;
using RepositoryInputModel = ChpokkWeb.Features.RepositoryManagement.RepositoryInputModel;
using FubuCore;

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
			if (Directory.Exists(repositoryPath)) {
				Directory.Delete(repositoryPath, true);
			}
			var credentials = model.Username.IsNotEmpty()? new Credentials() {Username = model.Username, Password = model.Password} : null;
			CloneGitRepository(repoUrl, repositoryPath, credentials);
			var projectUrl = _registry.UrlFor(new RepositoryInputModel() { RepositoryName = repositoryInfo.Name });
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}

		private static void CloneGitRepository(string repoUrl, string repositoryPath, Credentials credentials) {
			Repository.Clone(repoUrl, repositoryPath, credentials:credentials).Dispose();
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
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public class AppPathAwareInputModel {
		public string PhysicalApplicationPath { get; set; }		
	}
}