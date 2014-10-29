using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using Gallio.Framework;
using LibGit2Sharp;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuValidation;
using Shouldly;

namespace Chpokk.Tests.Git {
	[TestFixture]
	public class NewRemote : BaseCommandTest<GitRepositoryContext> {
		public string DEFAULT_REMOTE = "origin";
		[Test]
		public void ShouldAddARemote() {
			using (var repository = new Repository(Context.RepositoryRoot)) {
				repository.Network.Remotes.Count().ShouldBe(1);
			}
		}

		[Test]
		public void NewRemoteShouldBecomeDefault() {
			using (var repository = new Repository(Context.RepositoryRoot)) {
				repository.Head.Remote.Name.ShouldBe(DEFAULT_REMOTE);
			}			
		}

		public override void Act() {
			using (var repository = new Repository(Context.RepositoryRoot)) {
				repository.Network.Remotes.Add(DEFAULT_REMOTE, "https://ulu@appharbor.com/chpokk.git");
				repository.Branches.Update(repository.Head, updater => updater.Remote = DEFAULT_REMOTE, updater => updater.UpstreamBranch = "refs/heads/master");
			}
		}
	}
}
