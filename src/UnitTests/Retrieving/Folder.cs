using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Remotes;
using ChpokkWeb.Features.Remotes.DownloadZip;
using ChpokkWeb.Features.Remotes.Git.Push;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.Retrieving {
	[TestFixture]
	public class Folder : BaseQueryTest<RepositoryFolderContext, IEnumerable<MenuItem>> {
		[Test]
		public void CanNotPush() {
			Result.ShouldNotContain(item => item.Caption.ToLower().Contains("push"));
		}
		[Test]
		public void CanDownloadZip() {
			Result.ShouldContainItemOfType<DownloadZipMenuItem>();
		}


		public override IEnumerable<MenuItem> Act() {
			var controller = Context.Container.Get<RetrieveButtonsEndpoint>();
			var model = new RetrieveButtonsInputModel { RepositoryName = Context.REPO_NAME};
			return controller.DoIt(model).RetrieveActions;
		}
	}
}
