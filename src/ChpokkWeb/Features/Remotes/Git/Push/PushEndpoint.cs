using System;
using ChpokkWeb.Features.Remotes.Git.Init;
using ChpokkWeb.Features.Remotes.Git.Remotes;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure.Logging;
using FubuCore;
using FubuMVC.Core.Ajax;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Git.Push {
	public class PushEndpoint {
		private readonly RepositoryManager _manager;
		private readonly RemoteInfoProvider _remoteInfoProvider;
		private readonly GitInitializer _gitInitializer;
		private readonly GitCommitter _gitCommitter;
		public PushEndpoint(RepositoryManager manager, RemoteInfoProvider remoteInfoProvider, GitInitializer gitInitializer, GitCommitter gitCommitter) {
			_manager = manager;
			_remoteInfoProvider = remoteInfoProvider;
			_gitInitializer = gitInitializer;
			_gitCommitter = gitCommitter;
		}


		public PushResultModel Push(PushInputModel model) {
			var logger = SimpleLogger.CreateLogger(model.ConnectionId);
			var repositoryRoot = _manager.GetAbsoluteRepositoryPath(model.RepositoryName);
			EnsureInit(repositoryRoot);
			logger.Log("Autocommitting changes..");
			EnsureCommit(repositoryRoot);
			var credentials = model.Username.IsEmpty()? null: new UsernamePasswordCredentials() {Username = model.Username, Password = model.Password};
			var result = new PushResultModel { Success = true, ErrorMessage = String.Empty, PreviewLink = GetPreviewUrl(model.NewRemoteUrl)};
			var remoteName = GetRemoteName(model, repositoryRoot);
			using (var repo = new Repository(repositoryRoot)) {
				var remote = repo.Network.Remotes[remoteName];
				if (model.NewRemoteUrl.IsEmpty()) {
					result.PreviewLink = GetPreviewUrl(remote.Url);
				}
				var options = new PushOptions()
					{
						OnPackBuilderProgress = (stage, current, total) =>
						{
							logger.Log(stage.ToString() + ": " + current + "/" + total);
							return true;
						}, 
						OnPushTransferProgress = (current, total, bytes) =>
						{
							logger.Log("Transferring data: " + current + "/" + total);
							return true;
						}, 
						OnPushStatusError = error =>
						{
							result.Success = false;
							var errorMessage = error.Reference + ": " + error.Message + "/r";
							logger.Log("Error: " + errorMessage);
							result.ErrorMessage += errorMessage;
						},
						CredentialsProvider = (url, fromUrl, types) =>
						{
							return credentials;
						}
					};
				logger.Log("Pushing to the remote Git repository..");
				repo.Network.Push(remote, "HEAD", repo.Head.CanonicalName, options);
			}
			return result;
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

		private void EnsureInit(string repositoryRoot) {
			if (!_gitInitializer.GitRepositoryExistsIn(repositoryRoot)) {
				_gitInitializer.Init(repositoryRoot);
			}
		}

		private void EnsureCommit(string repositoryRoot) {
			var commitMessage = "Autocommit " + DateTime.Now.ToString();
			_gitCommitter.CommitAll(commitMessage, repositoryRoot);
		}

		private string GetPreviewUrl(string pushUrl) {
			if (pushUrl.IsEmpty()) {
				return null;
			}
			var uri = new Uri(pushUrl);
			if (uri.Host.Contains("azurewebsites.net")) {
				var newHost = uri.Host.Replace(".scm.", ".");
				return "http://" + newHost;
			}
			if (uri.Host == "appharbor.com") {
				var siteName = uri.AbsolutePath.TrimStart('/').Replace(".git", "");
				return "http://" + siteName + ".apphb.com/";
			}
			return null;
		}
	}

	public class PushResultModel {
		public bool Success { get; set; }
		public string PreviewLink { get; set; }
		public string ErrorMessage { get; set; }
	}
}