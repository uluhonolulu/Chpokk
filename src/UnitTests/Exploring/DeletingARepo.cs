using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using ChpokkWeb.Features.RepositoryManagement.DeleteRepository;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class DeletingARepo: BaseCommandTest<RepositoryFolderContext> {
		[Test]
		public void RepositoryFolderDoesNotExist() {
			Console.WriteLine(Context.RepositoryRoot);
			Directory.Exists(Context.RepositoryRoot).ShouldBe(false);
		}

		public override void Act() {
			var endpoint = Context.Container.Get<DeleteRepositoryEndpoint>();
			endpoint.DoIt(new DeleteRepositoryInputModel() {RepositoryName = Context.REPO_NAME});
		}
	}
}
