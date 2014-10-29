using System.Collections.Generic;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Remotes;
using ChpokkWeb.Features.Remotes.DownloadZip;
using ChpokkWeb.Infrastructure;
using MbUnit.Framework;
using Shouldly;
using UnitTests.Exploring;

namespace Chpokk.Tests.Retrieving {
	public class GitRepo : BaseQueryTest<GitRepositoryContext, IEnumerable<MenuItem>> {
		[Test]
		public void CanPush() {
			Result.ShouldContain(item => item.Caption.ToLower().Contains("push"));
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
