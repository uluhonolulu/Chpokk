using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Arractas;
using ChpokkWeb.Features.Storage;
using FubuCore;
using MbUnit.Framework;

namespace Chpokk.Tests.Amazon {
	[TestFixture]
	public class WaitTillRepositoryIsDownloaded : BaseCommandTest<OneFileOnAmazonContext> {
		DownloadSynchronizer _synchronizer = new DownloadSynchronizer();
		[Test]
		public void AllFilesShouldBeThere() {
			var repositoryFiles = Directory.GetFiles(Context.RepositoryRoot, "*.*", SearchOption.AllDirectories);
			Assert.Contains(repositoryFiles, Context.FileFullPath);
		}

		public override void Act() {
			var downloader = Context.Container.Get<Downloader>();
			var subFolder = Context.RepositoryRoot.PathRelativeTo(Context.AppRoot);
			Task.Run(() => {
							   _synchronizer._resetEvent.Reset();
				               downloader.DownloadAllFiles(Context.AppRoot, subFolder, (s, l) => Console.WriteLine(s));
							   _synchronizer._resetEvent.Set();
			});
			_synchronizer._resetEvent.WaitOne();

		}
	}
	public class DownloadSynchronizer {
		public ManualResetEvent _resetEvent = new ManualResetEvent(false);
		
	}
}
