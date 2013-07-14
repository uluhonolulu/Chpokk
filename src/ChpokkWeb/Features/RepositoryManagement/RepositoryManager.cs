using System.Collections.Generic;
using System.IO;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Remotes;
using ChpokkWeb.Features.Remotes.DownloadZip;
using ChpokkWeb.Features.Remotes.Push;
using ChpokkWeb.Infrastructure;
using FubuCore;
using System.Linq;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryManager {
		private readonly ISecurityContext _securityContext;
		private IEnumerable<IRetrievePolicy> _retrievePolicies; 
		public RepositoryManager(ISecurityContext securityContext, IEnumerable<IRetrievePolicy> retrievePolicies) {
			_securityContext = securityContext;
			_retrievePolicies = retrievePolicies;
		}

		public const string COMMON_REPOSITORY_FOLDER = "UserFiles";
		private const string anonymousFolder = "__anonymous__";
		// path for repository root, relative to AppRoot
		[NotNull]
		public string GetPathFor(string name) {
			return this.RepositoryFolder.AppendPathMyWay(name);
		}

		public RepositoryInfo GetClonedRepositoryInfo([NotNull] string url) {
			var name = url.GetFileNameUniversal().RemoveExtension();
			return GetRepositoryInfo(name);
		}

		[NotNull] 
		public RepositoryInfo GetRepositoryInfo([NotNull] string name) {
			return new RepositoryInfo(this.GetPathFor(name), name);
		}

		public bool RepositoryNameIsValid([NotNull] string name, string approot) {
			return this.GetRepositoryNames(approot).Contains(name);
		}


		[NotNull] 
		public IEnumerable<string> GetRepositoryNames(string approot) {
			var userFolder = approot.AppendPath(RepositoryFolder);
			if (!Directory.Exists(userFolder)) return Enumerable.Empty<string>();
			return Directory.EnumerateDirectories(userFolder).Select(Path.GetFileName);
 		}

		[NotNull]
		public string GetPhysicalFilePath([NotNull] BaseFileModel info) {
			var repositoryInfo = this.GetRepositoryInfo(info.RepositoryName);
			var repositoryRoot = info.PhysicalApplicationPath.AppendPath(repositoryInfo.Path);
			return repositoryRoot.AppendPathMyWay(info.PathRelativeToRepositoryRoot);
		}

		public string RepositoryFolder {
			get { 
				var userFolder = _securityContext.IsAuthenticated() ? _securityContext.CurrentIdentity.Name : anonymousFolder;
				return COMMON_REPOSITORY_FOLDER.AppendPath(userFolder);
			}
		}

		public MenuItem[] GetRetrieveActions(RepositoryInfo info, string approot) {
			var menuItems = new List<MenuItem>();
			foreach (var policy in _retrievePolicies) {
				if (policy.Matches(info, approot)) {
					var menuItemSource = policy as IMenuItemSource;
					if (menuItemSource != null) menuItems.Add(menuItemSource.GetMenuItem());
				}
			}
			return menuItems.ToArray();
		}
	}
}


