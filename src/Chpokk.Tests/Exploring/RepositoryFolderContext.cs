using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using LibGit2Sharp.Tests.TestHelpers;

namespace Chpokk.Tests.Exploring {
	public class RepositoryFolderContext : SimpleConfiguredContext, IDisposable {
		public string REPO_NAME = "Perka";
		public string RepositoryRoot { get; private set; }
		public override void Create() {
			base.Create();
			var repositoryManager = Container.Get<RepositoryManager>();
			var repositoryInfo = new RepositoryInfo("Repka", REPO_NAME);
			repositoryManager.Register(repositoryInfo);
			RepositoryRoot = Path.Combine(Path.GetFullPath(@".."), repositoryInfo.Path);
			if (!Directory.Exists(RepositoryRoot))
				Directory.CreateDirectory(RepositoryRoot);
		}

		public void Dispose() {
			DirectoryHelper.DeleteDirectory(RepositoryRoot);
		}
	}
}
