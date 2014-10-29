using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Git.Remotes {
	public class RemoteInfoProvider {
		public IEnumerable<string> GetRemoteNames(string repositoryRoot) {
			using (var repository = new Repository(repositoryRoot)) {
				var remotes = repository.Network.Remotes.ToArray(); //enumerate this before we're disposed
				return from remote in remotes select remote.Name;
			}
		}

		public string GetDefaultRemote(string repositoryRoot) {
			using (var repository = new Repository(repositoryRoot)) {
				if (repository.Head.Remote == null)
					return null;
				return repository.Head.Remote.Name;
			}
		}

		public void CreateRemote(string repositoryRoot, string name, string url) {
			using (var repository = new Repository(repositoryRoot)) {
				repository.Network.Remotes.Add(name, url);
				repository.Branches.Update(repository.Head, updater => updater.Remote = name,
										   updater => updater.UpstreamBranch = "refs/heads/master");
			}
		}
	}
}