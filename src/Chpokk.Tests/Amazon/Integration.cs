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
			//Console.WriteLine(path);
			Assert.IsTrue(Client.Exists(path));
		}

		public override void Act() {
			// add it to the cache by calling the browser
			var manager = Context.Container.Get<RepositoryManager>();

			// now abandon the session
			var backup = Context.Container.Get<Backup>();
			backup.Dispose();
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
