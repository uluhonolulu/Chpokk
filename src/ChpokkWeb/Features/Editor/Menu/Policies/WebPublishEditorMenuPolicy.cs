using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Navigation;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Editor.Menu.Policies {
	public class WebPublishEditorMenuPolicy :IEditorMenuPolicy{
		public bool Matches(string repositoryPath) {
			return RepositoryHasWebs(repositoryPath) && !(IsGitRepository(repositoryPath) && HasGitRemotes(repositoryPath));
		}

		private bool RepositoryHasWebs(string repositoryPath) {
			var webConfigPaths = Directory.EnumerateFiles(repositoryPath, "web.config", SearchOption.AllDirectories);
			return webConfigPaths.Any();
		}

		private bool IsGitRepository(string repositoryPath) {
			var path = repositoryPath.AppendPath(".git");
			return Directory.Exists(path);
		}

		private bool HasGitRemotes(string repositoryPath) {
			using (var repository = new Repository(repositoryPath)) {
				return repository.Network.Remotes.Any();
			}
		}

		public IEnumerable<MenuItem> GetMenuItems(string repositoryPath) {
			yield return new MenuItem() { Caption = "Preview your Web", Id = "publishWeb" };
		}
	}
}