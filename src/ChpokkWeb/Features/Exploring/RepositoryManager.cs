using System.Collections.Generic;
using System.IO;
using ChpokkWeb.Infrastructure;
using FubuCore;
using System.Linq;

namespace ChpokkWeb.Features.Exploring {
	public class RepositoryManager {

		private const string commonRepositoryFolder = "UserFiles";
		// path for repository root, relative to AppRoot
		[NotNull]
		public string GetPathFor(string name) {
			return commonRepositoryFolder.AppendPathMyWay(name);
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
			var userFolder = approot.AppendPath(commonRepositoryFolder);
			return Directory.EnumerateDirectories(userFolder).Select(Path.GetFileName);
 		}

		[NotNull]
		public string GetPhysicalFilePath([NotNull] BaseFileModel info) {
			var repositoryInfo = this.GetRepositoryInfo(info.RepositoryName);
			var repositoryRoot = info.PhysicalApplicationPath.AppendPath(repositoryInfo.Path);
			return repositoryRoot.AppendPathMyWay(info.PathRelativeToRepositoryRoot);
		}
	}
}