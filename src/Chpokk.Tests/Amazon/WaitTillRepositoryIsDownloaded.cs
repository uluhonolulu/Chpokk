using System;
using System.Collections.Generic;
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
		RestoreSynchronizer _synchronizer = new RestoreSynchronizer();
		[Test]
		public void AllFilesShouldBeThere() {
			var repositoryFiles = Directory.GetFiles(Context.RepositoryRoot, "*.*", SearchOption.AllDirectories);
			Assert.Contains(repositoryFiles, Context.FileFullPath);
		}

		public override void Act() {
			var downloader = Context.Container.Get<Downloader>();
			var subFolder = Context.RepositoryRoot.PathRelativeTo(Context.AppRoot);
			Task.Run(() => {
							   _synchronizer.RestoringStarted(subFolder);
				               downloader.DownloadAllFiles(Context.AppRoot, subFolder, (s, l) => Console.WriteLine(s));
							   _synchronizer.RestoringFinished(subFolder);
			});
			_synchronizer.WaitTillRestored(subFolder);

		}
	}
	public class RestoreSynchronizer {
		//public ManualResetEvent _resetEvent = new ManualResetEvent(false);
		IDictionary<string, ManualResetEvent> _resetEvents = new Dictionary<string, ManualResetEvent>();
		public void RestoringStarted(string path) {
			EnsureEvent(path);
			_resetEvents[path].Reset();
		}

		public void RestoringFinished(string path) {
			_resetEvents[path].Set();
		}

		public void WaitTillRestored(string path) {
			EnsureEvent(path);
			_resetEvents[path].WaitOne();
		}

		private void EnsureEvent(string path ) {
			if (!_resetEvents.ContainsKey(path)) {
				_resetEvents.Add(path, new ManualResetEvent(false));
			}
		}
	}
}
