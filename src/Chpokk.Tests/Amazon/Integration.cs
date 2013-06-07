using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Features.Storage;
using Emkay.S3;
using FubuMVC.Core;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using RepositoryInputModel = ChpokkWeb.Features.Exploring.RepositoryInputModel;

namespace Chpokk.Tests.Amazon {
	[TestFixture, RunOnWeb] //RunOnWeb is used for correct guessing of the app root
	public class Integration : BaseCommandTest<OneFileInRepositoryFolderContext> {
		[Test]
		public void ShouldUploadTheRepositoryAfterClosingTheSession() {
			var path = Context.FilePathRelativeToAppRoot;
			Assert.IsTrue(Client.Exists(path));
		}

		public override void Act() {
			// add it to the cache by calling the browser
			var input = new ChpokkWeb.Features.RepositoryManagement.RepositoryInputModel {Name = Context.REPO_NAME};
			var controller = Context.Container.Get<RepositoryController>();
			controller.Get(input);
			//cache["ulu/Perka"] = new RepositoryInfo("UserFiles/ulu/Perka", "Perka");
			// now abandon the session
			var cache = Context.Container.Get<RepositoryCache>();
			cache.Dispose();
			//var uploader = Context.Container.Get<Uploader>();
			//foreach (var repositoryInfo in cache) {
			//    uploader.PublishFolder(Path.Combine(Context.AppRoot, repositoryInfo.Path), Context.AppRoot);
			//}
			
		}

		[FixtureSetUp]
		public override void Arrange() {
			base.Arrange();
		}

		public override void CleanUp() {
			base.CleanUp();
			var path = Context.FilePathRelativeToAppRoot;
			if (Client.Exists(path)) {
				Client.DeleteObject("chpokk", path);
			}
		}

		private IS3Client Client {
			get { return Context.Container.Get<IS3Client>(); }
		}

	}
}
