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
			var credentials = model.Username.IsEmpty()? null: new UsernamePasswordCredentials() {Username = model.Username, Password = model.Password};
			var repositoryRoot = _manager.NewGetAbsolutePathFor(model.RepositoryName);
			var ajaxContinuation = AjaxContinuation.Successful();
			ajaxContinuation.ShouldRefresh = true;
			var remoteName = GetRemoteName(model, repositoryRoot);
			using (var repo = new Repository(repositoryRoot)) {
				var remote = repo.Network.Remotes[remoteName];
				var options = new PushOptions()
					{
						OnPackBuilderProgress = (stage, current, total) => true, 
						OnPushTransferProgress = (current, total, bytes) => true, //TODO: log this to screen
						OnPushStatusError = error =>
						{
							ajaxContinuation.Success = false;
							var errorMessage = error.Reference + ": " + error.Message + "/r";
							ajaxContinuation.Errors.Add(new AjaxError {message = errorMessage});
						},
						CredentialsProvider = (url, fromUrl, types) => credentials
					};
				repo.Network.Push(remote, "HEAD", repo.Head.CanonicalName, options);
			}
			return ajaxContinuation;
		}

		private string GetRemoteName(PushInputModel model, string repositoryRoot) {
			if (model.NewRemote.IsNotEmpty()) {
				_remoteInfoProvider.CreateRemote(repositoryRoot, model.NewRemote, model.NewRemoteUrl);
				return model.NewRemote;
			}
			if (model.Remote.IsEmpty()) { //for the basic push, we use the default remote
				return _remoteInfoProvider.GetDefaultRemote(repositoryRoot);
			}
			return model.Remote;
		}
	}
}