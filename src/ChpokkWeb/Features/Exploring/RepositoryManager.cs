using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using ChpokkWeb.Features.Storage;
using ChpokkWeb.Infrastructure;
using FubuCore;
using System.Linq;
using FubuCore.Util;
using FubuMVC.Core;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.Exploring {
	public class RepositoryManager {
		private readonly ISecurityContext _securityContext;
		public RepositoryManager(ISecurityContext securityContext) {
			_securityContext = securityContext;
		}

		private const string commonRepositoryFolder = "UserFiles";
		private const string anonymousFolder = "__anonymous__";
		// path for repository root, relative to AppRoot
		[NotNull]
		public string GetPathFor(string name) {
			return this.RepositoryFolder.AppendPathMyWay(name);
		}

		public RepositoryInfo GetClonedRepositoryInfo([NotNull] string url) {
			var name = url.GetFileNameUniversal().RemoveExtension();
			var path = GetPathFor(name);
			return new RepositoryInfo(path, name);
		}

		//[NotNull]
		//private readonly Dictionary<string, RepositoryInfo> _repositories = new Dictionary<string, RepositoryInfo>();

		//public void Register([NotNull] RepositoryInfo info) {
		//    _repositories[info.Name] = info;
		//}

		[NotNull] 
		public RepositoryInfo GetRepositoryInfo([NotNull] string name) {
			return new RepositoryInfo(this.GetPathFor(name), name);
		}

		public bool RepositoryNameIsValid([NotNull] string name, string approot) {
			return this.GetRepositoryNames(approot).Contains(name);
		}


		[NotNull] 
		public IEnumerable<string> GetRepositoryNames(string approot) {
			HttpContext.Current.Session["time"] = DateTime.Now.TimeOfDay;
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
				//if (HttpContext.Current != null) {
				//    //BLOODY HELL THIS SHIT DOESNT WORK AS EXPECTED!!!
				//    userFolder = HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated ? HttpContext.Current.User.Identity.Name : anonymousFolder;
				//}
				return commonRepositoryFolder.AppendPath(userFolder);
			}
		}

	}

	public class RepositoryCache : Cache<string, RepositoryInfo>, IDisposable  {
		private Backup _backup;
		private Uploader _uploader;
		private ApplicationSettings _settings;

		public RepositoryCache(Uploader uploader, ApplicationSettings settings) {
			_uploader = uploader;
			_settings = settings;
			AppRoot = _settings.GetApplicationFolder();
		}

		public string AppRoot { get; set; }

		public void Dispose() {
			//var appRoot = _httpContextBase.Request.ApplicationPath
			foreach (var repositoryInfo in this) {
				_uploader.PublishFolder(Path.Combine(AppRoot, repositoryInfo.Path), AppRoot);
			}
		}
	}

	internal class Backup {}
}



//SessionScoped is actually RepositoryCache -- and IDisposable, depends on Uploader through Backup