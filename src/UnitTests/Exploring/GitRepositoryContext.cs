using System.Threading;
using Chpokk.Tests.Exploring;
using LibGit2Sharp;

namespace UnitTests.Exploring {
	public class GitRepositoryContext: RepositoryFolderContext {
		public override void Create() {
			base.Create();
			Repository.Init(RepositoryRoot);
			Thread.Sleep(100);
		}
	}
}