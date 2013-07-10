﻿using System;
using System.IO;
using Emkay.S3;
using FubuCore;

namespace ChpokkWeb.Features.Storage {
	public class Downloader {
		private readonly IS3Client _client;

		public Downloader(IS3Client client) {
			_client = client;
		}

		public void DownloadAllFiles(string root, Action<string> onDownload = null) {
			var allFiles = _client.EnumerateChildren("chpokk");
			foreach (var file in allFiles) {
				var localPath = FileSystem.Combine(root, file);
				if (!localPath.EndsWith("/")) { 
					_client.DownloadFile("chpokk", file, localPath, -1);
					if (onDownload != null) {
						onDownload(file);
					}
				}
				
			}
		}
	}
}