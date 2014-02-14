using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Git.Push {
	public class PushController {
		private RepositoryManager _manager;
		public PushController(RepositoryManager manager) {
			_manager = manager;
		}


		public AjaxContinuation Push(PushInputModel model) {
			var credentials = model.Username.IsEmpty()? null: new LibGit2Sharp.Credentials {Username = model.Username, Password = model.Password};
			var path = _manager.NewGetAbsolutePathFor(model.RepositoryName);
			string errorMessage;
			var ajaxContinuation = AjaxContinuation.Successful();
			using (var repo = new Repository(path)) {
				var remote = repo.Network.Remotes["origin"];
				repo.Network.Push(remote, "refs/heads/master", error => {
					ajaxContinuation.Success = false;
					errorMessage = error.Reference + ": " + error.Message + "/r";
					ajaxContinuation.Errors.Add(new AjaxError { message = errorMessage });
				}, credentials);
			}
			return ajaxContinuation;
		}


	
	}
}