﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Threading.Tasks;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Remotes;
using ChpokkWeb.Features.Remotes.DownloadZip;
using ChpokkWeb.Features.Storage;
using ChpokkWeb.Infrastructure;
using ChpokkWeb.Infrastructure.Menu;
using FubuCore;
using System.Linq;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryManager {
		private readonly ISecurityContext _securityContext;
		private readonly IEnumerable<IRetrievePolicy> _retrievePolicies;
		private readonly IFileSystem _fileSystem;
		private readonly Backup _backup;
		private readonly IAppRootProvider _rootProvider;
		private readonly RemoteFileListCache _remoteFileListCache;

		public RepositoryManager(ISecurityContext securityContext, IEnumerable<IRetrievePolicy> retrievePolicies, IFileSystem fileSystem, IAppRootProvider rootProvider, Backup backup, RemoteFileListCache remoteFileListCache) {
			_securityContext = securityContext;
			_retrievePolicies = retrievePolicies;
			_fileSystem = fileSystem;
			_rootProvider = rootProvider;
			_backup = backup;
			_remoteFileListCache = remoteFileListCache;
			RegisterUserFolderForBackup();
		}

		private void RegisterUserFolderForBackup() {
			_backup.RegisterUserFolder(this.GetUserFolder());
		}

		private const string COMMON_USER_FOLDER = @"UserFiles";
		private const string REPOSITORY_FOLDER = "Repositories";
		private const string SVN_FOLDER = "svn";
		private const string POKK_FOLDER = "Pokk";
		private const string anonymousFolder = "__anonymous__";
		// path for repository root, relative to AppRoot
		[NotNull]
		public string GetPathFor(string name) {
			return this.RelativeUserFolder.AppendPath(REPOSITORY_FOLDER).AppendPathMyWay(name);
		}

		[NotNull]
		public string GetAbsolutePathFor(string repositoryName, string appRoot) {
			return Path.GetFullPath(AppRoot.AppendPath(this.GetPathFor(repositoryName)));
		}

		private string AppRoot {
			get { return _rootProvider.AppRoot; }
		}


		[NotNull]
		public string NewGetAbsolutePathFor(string repositoryName, string pathRelativeToRepositoryRoot) {
			if (pathRelativeToRepositoryRoot == null) {
				throw new ArgumentNullException("pathRelativeToRepositoryRoot");
			}
			return GetAbsoluteRepositoryPath(repositoryName).AppendPath(pathRelativeToRepositoryRoot);
		}
		[NotNull]
		public string GetAbsoluteRepositoryPath(string repositoryName) {
			return Path.GetFullPath(AppRoot.AppendPath(this.GetPathFor(repositoryName)));
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
			return this.GetRepositoryNames().Contains(name);
		}


		[NotNull] 
		public IEnumerable<string> GetRepositoryNames() {
			var repositoryFolder = GetRepositoryFolder();
			if (!Directory.Exists(repositoryFolder)) return GetRepositoryNamesFromStorage();
			return Directory.EnumerateDirectories(repositoryFolder).Select(Path.GetFileName);
		}

		public IEnumerable<string> GetRepositoryNamesFromStorage() {
			var repositoryFolder = GetRepositoryFolder();
			var storagePrefix = repositoryFolder.PathRelativeTo(AppRoot).Replace('\\', '/').MakeSureEndsWith("/");
			var fullRemotePaths = _remoteFileListCache.Paths.Where(s => s.StartsWith(storagePrefix)); //_client.EnumerateChildren("chpokk", storagePrefix);
			return (from path in fullRemotePaths select path.Substring(storagePrefix.Length).Split('/')[0]).Distinct();
		}

		public string GetUserFolder() {
			return AppRoot.AppendPath(RelativeUserFolder);
		}

		public string GetRepositoryFolder() {
			return GetUserFolder().AppendPath(REPOSITORY_FOLDER);
		}

		public string GetSvnFolder() {
			return GetUserFolder().AppendPath(SVN_FOLDER);
		}

		public void MoveFilesToRepositoryFolder() {
			var userFolder = GetUserFolder();
			if (!Directory.Exists(userFolder))
				return;
			var childFoldersOfUserFolder = Directory.EnumerateDirectories(userFolder, "*", SearchOption.TopDirectoryOnly);
			foreach (var childFolder in childFoldersOfUserFolder) {
				if (!childFolder.EndsWith(REPOSITORY_FOLDER) && !childFolder.EndsWith(POKK_FOLDER) && !childFolder.EndsWith(SVN_FOLDER)) {
					var targetFolder = childFolder.Replace(userFolder, GetRepositoryFolder());
					//_fileSystem.CreateDirectory(targetFolder);
					//Console.WriteLine("Moving {0} to {1}", childFolder, targetFolder);
					//for some reason, just moving directories throws
					_fileSystem.MoveFiles(childFolder, targetFolder);
					//let's ignore the folders for now
					//foreach (var directory in Directory.EnumerateDirectories(childFolder, "*", SearchOption.AllDirectories)) {
					//	Directory.Delete(directory, true);
					//}
					//Directory.Delete(childFolder, true);
				}
			}

		}

		[NotNull]
		public string GetPhysicalFilePath([NotNull] BaseFileInputModel info) {
			return NewGetAbsolutePathFor(info.RepositoryName, info.PathRelativeToRepositoryRoot);
		}

		private string RelativeUserFolder {
			get { 
				var userFolder = _securityContext.IsAuthenticated() ? _securityContext.CurrentIdentity.Name : anonymousFolder;
				return COMMON_USER_FOLDER.AppendPath(userFolder);
			}
		}

		public MenuItem[] GetRetrieveActions(RepositoryInfo info) {
			var menuItems = new List<MenuItem>();
			var repositoryRoot = this.GetAbsoluteRepositoryPath(info.Name);
			foreach (var policy in _retrievePolicies) {
				if (policy.Matches(repositoryRoot)) {
					var menuItemSource = policy as IMenuItemSource;
					if (menuItemSource != null) menuItems.Add(menuItemSource.GetMenuItem(repositoryRoot));
				}
			}
			return menuItems.ToArray();
		}

		public bool RepositoriesExist() {
			var userFolder = GetCommonFolder() ;
			return ChildDirectoriesExist(userFolder);
		}

		public string GetCommonFolder() {
			return Path.GetFullPath(AppRoot.AppendPath(COMMON_USER_FOLDER));
		}

		private bool ChildDirectoriesExist(string parent) {
			return _fileSystem.ChildDirectoriesFor(parent).Any();
		}




		public void EnsureAuthenticated() {
			if (!_securityContext.IsAuthenticated()) {
				throw new AuthenticationException("Your session has expired. Please reload this page to relogin.");
			}
			
		}
	}
}


