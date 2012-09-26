using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using LibGit2Sharp.Tests.TestHelpers;
using FubuCore;

namespace Chpokk.Tests.Exploring {
	public class RepositoryFolderContext : SimpleConfiguredContext, IDisposable {
		public readonly string REPO_NAME = "Perka";
		public string RepositoryRoot { get; private set; }

		public string RepoPath { get; private set; }

		public override void Create() {
			base.Create();
			var repositoryManager = Container.Get<RepositoryManager>();
			RepoPath = repositoryManager.GetPathFor(REPO_NAME);
			var repositoryInfo = new RepositoryInfo(RepoPath, REPO_NAME);
			repositoryManager.Register(repositoryInfo);
			RepositoryRoot = Path.Combine(AppRoot, repositoryInfo.Path);
			DirectoryHelper.DeleteSubdirectories(RepositoryRoot.ParentDirectory());
			if (!Directory.Exists(RepositoryRoot))
				Directory.CreateDirectory(RepositoryRoot);
		}

		public void Dispose() {
			DirectoryHelper.DeleteDirectory(RepositoryRoot);
		}
	}
}
