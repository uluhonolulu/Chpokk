using System.Collections.Generic;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Exploring {
	public class RepositoryManager {
		public RepositoryManager() {
			// for local testing
			var name = "Chpokk-SampleSol";
			var path = commonRepositoryFolder.AppendPathMyWay(name);
			this.Register(new RepositoryInfo(path, name));
		}
		private const string commonRepositoryFolder = "UserFiles";
		public RepositoryInfo GetClonedRepositoryInfo([NotNull] string url) {
			var name = url.GetFileNameUniversal().RemoveExtension();
			var path = commonRepositoryFolder.AppendPathMyWay(name);
			return new RepositoryInfo(path, name);
		}

		[NotNull]
		private readonly Dictionary<string, RepositoryInfo> _repositories = new Dictionary<string, RepositoryInfo>();

		public void Register([NotNull] RepositoryInfo info) {
			_repositories[info.Name] = info;
		}

		[NotNull] 
		public RepositoryInfo GetRepositoryInfo([NotNull] string name) {
			return _repositories[name];
		}
		public bool RepositoryNameIsValid([NotNull] string name) {
			return _repositories.ContainsKey(name);
		}
	}
}