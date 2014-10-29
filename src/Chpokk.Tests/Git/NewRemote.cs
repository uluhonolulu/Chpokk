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
		[Test]
		public void ShouldAddARemote() {
			using (var repository = new Repository(Context.RepositoryRoot)) {
				repository.Network.Remotes.Count().ShouldBe(1);
			}
		}

		public override void Act() {}
	}
}
