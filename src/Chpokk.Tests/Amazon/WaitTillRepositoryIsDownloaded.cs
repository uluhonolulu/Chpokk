using System;
using System.IO;
using Arractas;
using ChpokkWeb.Features.Storage;
using FubuCore;
using MbUnit.Framework;

namespace Chpokk.Tests.Amazon {
	[TestFixture]
	public class WaitTillRepositoryIsDownloaded : BaseCommandTest<OneFileOnAmazonContext> {
		[Test]
		public void AllFilesShouldBeThere() {
			var repositoryFiles = Directory.GetFiles(Context.RepositoryRoot, "*.*", SearchOption.AllDirectories);
			Assert.Contains(repositoryFiles, Context.FileFullPath);
		}

		public override void Act() {
			var downloader = Context.Container.Get<Downloader>();
			var subFolder = Context.RepositoryRoot.PathRelativeTo(Context.AppRoot);
			Console.WriteLine("Looking in " + subFolder);
			downloader.DownloadAllFiles(Context.AppRoot, subFolder, (s, l) => Console.WriteLine(s));
			
		}
	}
}
