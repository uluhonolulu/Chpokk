using System.IO;
using ChpokkWeb.Features.Repa;
using FubuMVC.Core;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;
using LibGit2Sharp;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Remotes {
	public class CloneController {

		private IUrlRegistry _registry;
		public CloneController(IUrlRegistry registry) {
			_registry = registry;
		}

		[JsonEndpoint]
		public AjaxContinuation CloneRepo(CloneInputModel model) {
			CloneRepository(model).Dispose();
			var projectUrl = _registry.UrlFor(new RepositoryInputModel() {Name = RepositoryInfo.Name});
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}

		private static Repository CloneRepository(CloneInputModel input) {
			var repositoryPath = Path.Combine(input.PhysicalApplicationPath, RepositoryInfo.Path);
			var repository = Repository.Clone(input.RepoUrl, repositoryPath);
			var master = repository.Branches["master"];
			repository.Checkout(master);
			return repository;
		}
	}

	public class CloneInputModel {
		public string RepoUrl { get; set; }
		public string PhysicalApplicationPath { get; set; }
	}
}