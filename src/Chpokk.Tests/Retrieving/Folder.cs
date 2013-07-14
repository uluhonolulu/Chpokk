using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Remotes.DownloadZip;
using ChpokkWeb.Features.Remotes.Push;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Retrieving {
	[TestFixture]
	public class Folder : BaseQueryTest<RepositoryFolderContext, IEnumerable<MenuItem>> {
		[Test]
		public void CanNotPush() {
			Result.ShouldNotContainItemOfType<PushMenuItem>();
		}
		[Test]
		public void CanDownloadZip() {
			Result.ShouldContainItemOfType<DownloadZipMenuItem>();
		}


		public override IEnumerable<MenuItem> Act() {
			var controller = Context.Container.Get<RepositoryEndpoint>();
			var model = new RepositoryInputModel { RepositoryName = Context.REPO_NAME, PhysicalApplicationPath = Context.AppRoot};
			return controller.Get(model).RetrieveActions;
		}
	}
}
