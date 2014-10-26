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
			using (var repository = new Repository(Context.RepositoryRoot)) {
				var origin = repository.Network.Remotes.Add("origin", "http://www");
				repository.Head.Remote.ShouldNotBe(null);
			}

			using (var repository = new Repository(Context.RepositoryRoot)) {
				repository.Branches["master"].ShouldNotBe(null);
				var remotes = repository.Network.Remotes;
				return new RemoteListModel
					{
						Remotes = from remote in remotes select remote.Name,
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
