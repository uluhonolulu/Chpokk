using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Arractas;
using ChpokkWeb.Features.Storage;
using FubuCore;
using MbUnit.Framework;
using UnitTests.Amazon;

namespace Chpokk.Tests.Amazon {
	[TestFixture]
	public class WaitTillRepositoryIsDownloaded : BaseCommandTest<OneFileOnAmazonContext> {
		[Test]
		public void AllFilesShouldBeThere() {
			var repositoryFiles = Directory.GetFiles(Context.RepositoryRoot, "*.*", SearchOption.AllDirectories);
			Assert.Contains(repositoryFiles, Context.FileFullPath);
		}

		public override void Act() {
			var synchronizer = Context.Container.Get<RestoreSynchronizer>();
			var downloader = Context.Container.Get<Downloader>();
			var subFolder = Context.RepositoryRoot.PathRelativeTo(Context.AppRoot);
			Task.Run(() => {
				               downloader.DownloadAllFiles(Context.AppRoot, subFolder, (s, l) => Console.WriteLine(s));
			});
			Thread.Sleep(100);// simulate browsing to a different page
			synchronizer.WaitTillRestored(Context.RepositoryRoot);

		}
	}
}
