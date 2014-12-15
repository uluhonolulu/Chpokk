using System;
using System.IO;
using Arractas;
using Chpokk.Tests.Amazon;
using ChpokkWeb.Features.Storage;
using FubuCore;
using MbUnit.Framework;

namespace UnitTests.Amazon {
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
