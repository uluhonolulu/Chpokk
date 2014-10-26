using System;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using Gallio.Framework;
using LibGit2Sharp;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using StoryTeller;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.Git {
	[TestFixture]
	public class Remotes : BaseQueryTest<GitRepositoryWithOneRemoteContext, RemoteListModel> {

		[Test]
		public void ShouldHaveOneRemote() {
			Result.Remotes.Count().ShouldBe(1);
		}

		[Test]
		public void DefaultRemoteShouldBeOrigin() {
			Result.DefaultRemote.ShouldBe(Context.DEFAULT_REMOTE);
		}

		public override RemoteListModel Act() {
			var repositoryRoot = Context.RepositoryRoot;
			return Context.Container.Get<RemoteListEndpoint>().GetRemoteInfo(repositoryRoot);
		}
	}

	public class GitRepositoryWithOneRemoteContext: GitRepositoryContext {
		public string DEFAULT_REMOTE = "origin";
		public override void Create() {
			base.Create();
			using (var repository = new Repository(this.RepositoryRoot)) {
				repository.Network.Remotes.Add(DEFAULT_REMOTE, "https://ulu@appharbor.com/chpokk.git");
				repository.Branches.Update(repository.Head, updater => updater.Remote = DEFAULT_REMOTE, updater => updater.UpstreamBranch = "refs/heads/master");
			}
		}
	}
}
