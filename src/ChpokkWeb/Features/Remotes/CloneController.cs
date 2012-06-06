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
		public CloneController(IUrlRegistry registry) {
			_registry = registry;
		}

		[JsonEndpoint]
		public AjaxContinuation CloneRepo(CloneInputModel model) {
			try {
				CloneRepository(model);
			}
			catch (System.Exception exception) {
				Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Error(exception));
				return new AjaxContinuation().ForException(exception);
			} 
				var projectUrl = _registry.UrlFor(new ProjectInputModel() { Name = RepositoryInfo.Name });
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}

		private static void CloneRepository(CloneInputModel input) {
			var repositoryPath = Path.Combine(input.PhysicalApplicationPath, RepositoryInfo.Path);
			var repository = Repository.Clone(input.RepoUrl, repositoryPath);
			var master = repository.Branches["master"];
			repository.Checkout(master);
			repository.Dispose();
		}
	}

	public class CloneInputModel : IDontNeedActionsModel {
		public string RepoUrl { get; set; }
		public string PhysicalApplicationPath { get; set; }
	}
}