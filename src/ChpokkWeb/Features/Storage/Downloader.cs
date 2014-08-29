using System;
using System.Collections.Generic;
using System.IO;
using Emkay.S3;
using FubuCore;
using System.Linq;

namespace ChpokkWeb.Features.Storage {
	public class Downloader {
		private readonly IS3Client _client;

		public Downloader(IS3Client client) {
			_client = client;
		}

		public void DownloadAllFiles(string root, string subFolder = null, Action<string, string> onDownload = null) {
			var prefix = subFolder != null? subFolder.Replace('\\', '/'): string.Empty;
			IEnumerable<string> allFiles = _client.EnumerateChildren("chpokk", prefix); 
			foreach (var file in allFiles) {
				var localPath = FileSystem.Combine(root, file);
				if (!localPath.EndsWith("/")) { 
					_client.DownloadFile("chpokk", file, localPath, -1);
					if (onDownload != null) {
						onDownload(file, localPath);
					}
				}
				
			}
		}
	}
}