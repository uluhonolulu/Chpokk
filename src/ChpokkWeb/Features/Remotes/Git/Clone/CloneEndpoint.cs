using System;
using System.IO;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Core;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;
using LibGit2Sharp;
using ChpokkWeb.Infrastructure;
using FubuCore;
using SharpSvn;
using SharpSvn.Remote;
using SharpSvn.Security;

namespace ChpokkWeb.Features.Remotes.Git.Clone {
	public class CloneEndpoint {

		private readonly IUrlRegistry _registry;
		private readonly RepositoryManager _repositoryManager;
		public CloneEndpoint(IUrlRegistry registry, RepositoryManager repositoryManager) {
			_registry = registry;
			_repositoryManager = repositoryManager;
		}

		[JsonEndpoint]
		public AjaxContinuation CloneRepository(CloneInputModel model) {
			var repoUrl = model.RepoUrl;
			if (repoUrl == null) {
				throw new ArgumentException("Url shouldn't be null", "Repository URL");
			}
			var repositoryRoot = GetRepositoryRoot(model);
			var repositoryName = repositoryRoot.GetFileNameUniversal().RemoveExtension();
			var repositoryPath = _repositoryManager.NewGetAbsolutePathFor(repositoryName);
			if (Directory.Exists(repositoryPath)) {
				Directory.Delete(repositoryPath, true);
			}
			switch (model.RepositoryType) {
				case CloneInputModel.RepositoryTypes.Git:
					CloneGitRepository(model, repositoryPath); break;
				case CloneInputModel.RepositoryTypes.SVN:
					CheckoutSvnRepository(model, repositoryPath); break;
			}
			
			var projectUrl = _registry.UrlFor(new RepositoryInputModel() { RepositoryName = repositoryName });
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}

		private void CheckoutSvnRepository(CloneInputModel model, string repositoryPath) {
			using (var client = new SvnClient()) {
				if (model.Username.IsNotEmpty()) {
					client.Authentication.DefaultCredentials = new System.Net.NetworkCredential(model.Username, model.Password);
				}
				
				client.Authentication.SslServerTrustHandlers += delegate(object sender, SvnSslServerTrustEventArgs e) {
					e.AcceptedFailures = e.Failures;
					e.Save = true; // Save acceptance to authentication store
				};
				client.CheckOut(new SvnUriTarget(model.RepoUrl), repositoryPath);
				
			}
		}

		private static void CloneGitRepository(CloneInputModel model, string repositoryPath) {
			var credentials = model.Username.IsNotEmpty()
				                  ? new Credentials() {Username = model.Username, Password = model.Password}
				                  : null;
			Repository.Clone(model.RepoUrl, repositoryPath, credentials:credentials);
		}

		private string GetRepositoryRoot(CloneInputModel model) {
			switch (model.RepositoryType) {
				case CloneInputModel.RepositoryTypes.Git:
					return model.RepoUrl;
				case CloneInputModel.RepositoryTypes.SVN:
					Uri uri;
					new SvnRemoteSession(new Uri(model.RepoUrl)).GetRepositoryRoot(out uri);
					return uri.ToString().GetFileNameUniversal();
			}
			return null;
		}


	}

	public class CloneInputModel : IDontNeedActionsModel {
		public string RepoUrl { get; set; }
		public string PhysicalApplicationPath { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string _repositoryType { get; set; }


		public RepositoryTypes RepositoryType {
			get {
				if (_repositoryType == git) {
					return RepositoryTypes.Git;
				}
				if (_repositoryType == svn) {
					return RepositoryTypes.SVN;
				}
				throw new NotSupportedException();
			}
		}

		private const string git = "git";
		private const string svn = "svn";

		public enum RepositoryTypes {
			Git,
			SVN
		}
	}

	public class AppPathAwareInputModel {
		public string PhysicalApplicationPath { get; set; }		
	}
}