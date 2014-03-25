using System;
using System.IO;
using System.Text;
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
using System.Linq;

namespace ChpokkWeb.Features.Remotes.Git.Clone {
	public class CloneEndpoint {

		private readonly IUrlRegistry _registry;
		private readonly RepositoryManager _repositoryManager;
		private readonly CredentialsCache _credentialsCache;
		private IFileSystem _fileSystem;
		public CloneEndpoint(IUrlRegistry registry, RepositoryManager repositoryManager, CredentialsCache credentialsCache, IFileSystem fileSystem) {
			_registry = registry;
			_repositoryManager = repositoryManager;
			_credentialsCache = credentialsCache;
			_fileSystem = fileSystem;
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

			//save to cache
			if (model.Username.IsNotEmpty()) {
				_credentialsCache.Add(repositoryPath, new Credentials{UserName = model.Username, Password = model.Password});
			}
			
			var projectUrl = _registry.UrlFor(new RepositoryInputModel() { RepositoryName = repositoryName });
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}

		private void CheckoutSvnRepository(CloneInputModel model, string repositoryPath) {
			using (var client = new SvnClient()) {
				if (model.Username.IsNotEmpty()) {
					client.Authentication.UserNamePasswordHandlers -= SvnAuthentication.SubversionWindowsUserNamePasswordHandler;
					client.Authentication.UserNamePasswordHandlers += (sender, args) =>
					{
						args.UserName = model.Username;
						args.Password = model.Password;
						args.Save = true;
					};
				}
				
				client.Authentication.SslServerTrustHandlers += delegate(object sender, SvnSslServerTrustEventArgs e) {
					e.AcceptedFailures = e.Failures;
					e.Save = true; // Save acceptance to authentication store
				};

				_fileSystem.CreateDirectory(_repositoryManager.GetSvnFolder());
				if (!Directory.Exists(_repositoryManager.GetSvnFolder())) {
					throw new ApplicationException("Couldn't create folder " + _repositoryManager.GetSvnFolder());
				}
				client.LoadConfiguration(_repositoryManager.GetSvnFolder(), true);
				client.CheckOut(new SvnUriTarget(model.RepoUrl), repositoryPath);
				
			}
		}

		private static void CloneGitRepository(CloneInputModel model, string repositoryPath) {
			LibGit2Sharp.Credentials credentials = model.Username.IsNotEmpty()
								  ? new LibGit2Sharp.Credentials() { Username = model.Username, Password = model.Password }
				                  : null;
			Repository.Clone(model.RepoUrl, repositoryPath, credentials:credentials);
		}

		private string GetRepositoryRoot(CloneInputModel model) {
			switch (model.RepositoryType) {
				case CloneInputModel.RepositoryTypes.Git:
					return model.RepoUrl;
				case CloneInputModel.RepositoryTypes.SVN:
					Uri uri = null;
					var remoteSession = new SvnRemoteSession();
					remoteSession.Authentication.ForceCredentials(model.Username, model.Password);
					remoteSession.Open(new Uri(model.RepoUrl));
					remoteSession.GetRepositoryRoot(out uri);
					return uri.ToString().GetFileNameUniversal();
			}
			return null;
		}

		private string GetAssemblies() {
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var builder = new StringBuilder();
			foreach (var assembly in assemblies.Where(assembly => assembly.GetName().ProcessorArchitecture.ToString() != "MSIL")) {
				builder.AppendLine(assembly.FullName + ": " + assembly.GetName().ProcessorArchitecture);
			}
			return builder.ToString();
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