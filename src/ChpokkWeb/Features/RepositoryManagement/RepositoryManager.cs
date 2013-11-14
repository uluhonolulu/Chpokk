using System.Collections.Generic;
using System.IO;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Remotes;
using ChpokkWeb.Features.Remotes.DownloadZip;
using ChpokkWeb.Features.Remotes.Push;
using ChpokkWeb.Features.Storage;
using ChpokkWeb.Infrastructure;
using FubuCore;
using System.Linq;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryManager {
		private readonly ISecurityContext _securityContext;
		private readonly IEnumerable<IRetrievePolicy> _retrievePolicies;
		private readonly FubuCore.FileSystem _fileSystem;
		private readonly Downloader _downloader;
		public RepositoryManager(ISecurityContext securityContext, IEnumerable<IRetrievePolicy> retrievePolicies, FubuCore.FileSystem fileSystem, Downloader downloader) {
			_securityContext = securityContext;
			_retrievePolicies = retrievePolicies;
			_fileSystem = fileSystem;
			_downloader = downloader;
		}

		private const string COMMON_REPOSITORY_FOLDER = @"..\UserFiles";
		private const string anonymousFolder = "__anonymous__";
		// path for repository root, relative to AppRoot
		[NotNull]
		public string GetPathFor(string name) {
			return this.RepositoryFolder.AppendPathMyWay(name);
		}

		[NotNull]
		public string GetAbsolutePathFor(string repositoryName, string appRoot) {
			return Path.GetFullPath(appRoot.AppendPath(this.GetPathFor(repositoryName))) ;
		}

		[NotNull]
		public string GetAbsolutePathFor(string repositoryName, string appRoot, string pathRelativeToRepositoryRoot) {
			return GetAbsolutePathFor(repositoryName, appRoot).AppendPath(pathRelativeToRepositoryRoot);
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
			var userFolder = GetUserFolder(approot);
			if (!Directory.Exists(userFolder)) return Enumerable.Empty<string>();
			return Directory.EnumerateDirectories(userFolder).Select(Path.GetFileName);
 		}

		public string GetUserFolder(string approot) {
			return approot.AppendPath(RepositoryFolder);
		}

		[NotNull]
		public string GetPhysicalFilePath([NotNull] BaseFileInputModel info) {
			return GetAbsolutePathFor(info.RepositoryName, info.PhysicalApplicationPath, info.PathRelativeToRepositoryRoot);
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

		public bool RepositoriesExist(string appRoot) {
			var userFolder = GetCommonFolder(appRoot) ;
			return ChildDirectoriesExist(userFolder);
		}

		public string GetCommonFolder(string appRoot) {
			return Path.GetFullPath(appRoot.AppendPath(COMMON_REPOSITORY_FOLDER));
		}

		private bool ChildDirectoriesExist(string parent) {
			return _fileSystem.ChildDirectoriesFor(parent).Any();
		}

		public bool RepositoriesOfCurrentUserExist(string appRoot) {
			return GetRepositoryNames(appRoot).Any();
		}

		public void RestoreFilesForCurrentUser(string appRoot) {
			var root = GetCommonFolder(appRoot).ParentDirectory(); //we need the parent cause we already have "UserFiles" on the remote
			var subFolder = RepositoryFolder.Substring(3); //HACK: we need to remove the "..\" from the start
			_downloader.DownloadAllFiles(root, subFolder);
		}

		
	}
}


