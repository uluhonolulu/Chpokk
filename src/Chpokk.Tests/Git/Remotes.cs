using System;
using System.Collections.Generic;
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
	public class Remotes: BaseQueryTest<GitRepositoryContext, RemoteListModel> {
		private const string DEFAULT_REMOTE = "origin";

		[Test]
		public void ShouldHaveOneRemote() {
			Result.Remotes.Count().ShouldBe(1);
		}

		[Test]
		public void DefaultRemoteShouldBeOrigin() {
			Result.DefaultRemote.ShouldBe(DEFAULT_REMOTE);
		}

		public override RemoteListModel Act() {
			using (var repository = new Repository(Context.RepositoryRoot)) {
				var origin = repository.Network.Remotes.Add(DEFAULT_REMOTE, "https://ulu@appharbor.com/chpokk.git");
				//repository.Head.Remote.ShouldNotBe(null);
				repository.Branches.Update(repository.Head, updater => updater.Remote = DEFAULT_REMOTE, updater => updater.UpstreamBranch = "refs/heads/master");
			}

			using (var repository = new Repository(Context.RepositoryRoot)) {
				repository.Head.ShouldNotBe(null);
				repository.Head.Remote.ShouldNotBe(null);
				var remotes = repository.Network.Remotes;
				return new RemoteListModel
					{
						Remotes = (from remote in remotes select remote.Name).ToArray(), //enumerate this before we're disposed
						DefaultRemote = repository.Head.Remote.Name
					};
			}
		}
	}

	public class RemoteListModel {
		public IEnumerable<string> Remotes { get; set; }
		public string DefaultRemote { get; set; }
	}
}
