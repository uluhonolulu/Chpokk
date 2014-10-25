using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Remotes.Git.Init;
using Gallio.Framework;
using LibGit2Sharp;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;
using Shouldly;

namespace Chpokk.Tests.GitHub {
	[TestFixture]
	public class GitInit: BaseCommandTest<RepositoryFolderContext> {
		[Test]
		public void GitFolderIsCreated() {
			Directory.Exists(Context.RepositoryRoot.AppendPath(".git")).ShouldBe(true);
		}

		public override void Act() {
			var repositoryRoot = Context.RepositoryRoot;
			Context.Container.Get<GitInitializer>().Init(repositoryRoot);
		}
	}
}
