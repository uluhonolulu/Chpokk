using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Emkay.S3;
using FubuCore;
using System.Linq;

namespace ChpokkWeb.Features.Storage {
	public class Downloader {
		private readonly IS3Client _client;
		private readonly RestoreSynchronizer _restoreSynchronizer;


		public Downloader(IS3Client client, RestoreSynchronizer restoreSynchronizer) {
			_client = client;
			_restoreSynchronizer = restoreSynchronizer;
		}

		public void DownloadAllFiles(string root, string subFolder = null, Action<string, string> onDownload = null) {
			var folderPath = root.AppendPath(subFolder);
			_restoreSynchronizer.RestoringStarted(folderPath);
			var prefix = subFolder != null? subFolder.Replace('\\', '/'): string.Empty;
			IEnumerable<string> allFiles = _client.EnumerateChildren("chpokk", prefix); 
			foreach (var file in allFiles) {
				var localPath = FileSystem.Combine(root, file);
				if (!localPath.EndsWith("/") && !File.Exists(localPath)) { 
					_client.DownloadFile("chpokk", file, localPath);
					if (onDownload != null) {
						onDownload(file, localPath);
					}
				}
				
			}
			//tell everybody that we've downloaded it
			_restoreSynchronizer.RestoringFinished(folderPath);
		}
	}
}