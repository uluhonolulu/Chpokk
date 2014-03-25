using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Remotes.Git.Clone;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Core.Ajax;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace SvnTests {
	[TestFixture]
	public class CanCheckOut : BaseCommandTest<SimpleAuthenticatedContext> {
		[Test]
		public void CredentialFolderExists() {
			var manager = Context.Container.Get<RepositoryManager>();
			Directory.Exists(manager.GetSvnFolder()).ShouldBe(true);
		}

		public override void Act() {
			var endpoint = Context.Container.Get<CloneEndpoint>();
			var continuation = endpoint.CloneRepository(new CloneInputModel()
				{
					//_repositoryType = "svn", RepoUrl = "https://sharpsvn.open.collab.net/svn/sharpsvn/trunk", Username = "guest"
					_repositoryType = "svn",
					RepoUrl = "http://178.63.130.238:8080/svn/dis/trunk",
					Username = "drzitz",
					Password = "iddqd710"
				});
		}
	}
}
