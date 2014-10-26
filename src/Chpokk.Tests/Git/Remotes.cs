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
		[Test]
		public void ShouldHaveOneRemote() {
			Result.Remotes.Count().ShouldBe(1);
		}

		public override RemoteListModel Act() {
			var remoteName = "origin";
			using (var repository = new Repository(Context.RepositoryRoot)) {
				var origin = repository.Network.Remotes.Add(remoteName, "https://ulu@appharbor.com/chpokk.git");
				//repository.Head.Remote.ShouldNotBe(null);
				repository.Branches.Update(repository.Head, updater => updater.Remote = remoteName, updater => updater.UpstreamBranch = "refs/heads/master");
			}

			using (var repository = new Repository(Context.RepositoryRoot)) {
				repository.Head.ShouldNotBe(null);
				var remotes = repository.Network.Remotes;
				return new RemoteListModel
					{
						Remotes = (from remote in remotes select remote.Name).ToArray(), //enumerate this before we're disposed
						DefaultRemote = remoteName //repository.Head.Remote.Name
					};
			}
		}
	}

	public class RemoteListModel {
		public IEnumerable<string> Remotes { get; set; }
		public string DefaultRemote { get; set; }
	}
}
