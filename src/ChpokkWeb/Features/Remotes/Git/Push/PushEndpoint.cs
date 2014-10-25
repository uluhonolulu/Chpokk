using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Git.Push {
	public class PushEndpoint {
		private readonly RepositoryManager _manager;
		public PushEndpoint(RepositoryManager manager) {
			_manager = manager;
		}


		public AjaxContinuation Push(PushInputModel model) {
			var credentials = model.Username.IsEmpty()? null: new LibGit2Sharp.Credentials {Username = model.Username, Password = model.Password};
			var path = _manager.NewGetAbsolutePathFor(model.RepositoryName);
			var ajaxContinuation = AjaxContinuation.Successful();
			using (var repo = new Repository(path)) {
				var remote = repo.Network.Remotes["origin"];
				repo.Network.Push(remote, "refs/heads/master", error => {
					ajaxContinuation.Success = false;
					var errorMessage = error.Reference + ": " + error.Message + "/r";
					ajaxContinuation.Errors.Add(new AjaxError { message = errorMessage });
				}, credentials);
			}
			return ajaxContinuation;
		}


	
	}
}