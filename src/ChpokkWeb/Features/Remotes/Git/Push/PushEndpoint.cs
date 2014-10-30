using ChpokkWeb.Features.Remotes.Git.Remotes;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Git.Push {
	public class PushEndpoint {
		private readonly RepositoryManager _manager;
		private RemoteInfoProvider _remoteInfoProvider;
		public PushEndpoint(RepositoryManager manager, RemoteInfoProvider remoteInfoProvider) {
			_manager = manager;
			_remoteInfoProvider = remoteInfoProvider;
		}


		public AjaxContinuation Push(PushInputModel model) {
			var credentials = model.Username.IsEmpty()? null: new LibGit2Sharp.Credentials {Username = model.Username, Password = model.Password};
			var repositoryRoot = _manager.NewGetAbsolutePathFor(model.RepositoryName);
			var ajaxContinuation = AjaxContinuation.Successful();
			using (var repo = new Repository(repositoryRoot)) {
				var remoteName = GetRemoteName(model, repositoryRoot);
				var remote = repo.Network.Remotes[remoteName];
				repo.Network.Push(remote, "refs/heads/master", error => {
					ajaxContinuation.Success = false;
					var errorMessage = error.Reference + ": " + error.Message + "/r";
					ajaxContinuation.Errors.Add(new AjaxError { message = errorMessage });
				}, credentials);
			}
			return ajaxContinuation;
		}

		private string GetRemoteName(PushInputModel model, string repositoryRoot) {
			if (model.NewRemote.IsNotEmpty()) {
				_remoteInfoProvider.CreateRemote(repositoryRoot, model.NewRemote, model.NewRemoteUrl);
				return model.NewRemote;
			}
			return model.Remote;
		}
	}
}