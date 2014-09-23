using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using ChpokkWeb.Features.Storage;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Amazon {
	[TestFixture]
	public class Downloading: BaseCommandTest<OneFileOnAmazonContext> {


		[Test]
		public void FileFromAmazonShouldAppearInUserFiles() {
			var repositoryFiles = Directory.GetFiles(Context.RepositoryRoot, "*.*", SearchOption.AllDirectories);
			Assert.Contains(repositoryFiles, Context.FileFullPath); 
		}

		public override void Act() {
			var downloader = Context.Container.Get<Downloader>();
			var subFolder = Context.RepositoryRoot.PathRelativeTo(Context.AppRoot);
			Console.WriteLine("Looking in " + subFolder);
			downloader.DownloadAllFiles(Context.AppRoot, subFolder, (s,l) => Console.WriteLine(s));
		}
	}
}
