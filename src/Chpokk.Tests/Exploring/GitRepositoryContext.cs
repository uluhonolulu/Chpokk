using System.Threading;
using LibGit2Sharp;

namespace Chpokk.Tests.Exploring {
	public class GitRepositoryContext: RepositoryFolderContext {
		public override void Create() {
			base.Create();
			Repository.Init(RepositoryRoot);
			Thread.Sleep(100);
		}
	}
}