using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
		public RepositoryInfo GetClonedRepositoryInfo(string url) {
			var name = url.GetFileNameUniversal().RemoveExtension();
			var path = commonRepositoryFolder.AppendPathMyWay(name);
			return new RepositoryInfo(path, name);
		}

		[NotNull]
		private readonly Dictionary<string, RepositoryInfo> _repositories = new Dictionary<string, RepositoryInfo>();

		public void Register([NotNull] RepositoryInfo info) {
			_repositories[info.Name] = info;
		}
		public RepositoryInfo GetRepositoryInfo(string name) {
			return _repositories[name];
		}
		public bool RepositoryNameIsValid(string name) {
			return _repositories.ContainsKey(name);
		}
	}
}