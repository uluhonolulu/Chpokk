﻿using System;
using System.IO;
using Emkay.S3;
using FubuCore;

namespace Chpokk.Tests.Amazon {
	public class Uploader {
		private IS3Client _client;
		public Uploader(IS3Client client) {
			_client = client;
		}


		public void PublishFolder(string path, string appRoot) {
			foreach (var filePath in Directory.GetFiles(path)) {
				var pathRelativetoAppRoot = filePath.PathRelativeTo(appRoot);
				var key = pathRelativetoAppRoot.Replace('\\', '/');
				_client.PutFile("chpokk", key, filePath, true, 0);
				Console.WriteLine(key); // full path here
			}

			foreach (var directory in Directory.GetDirectories(path)) {
				PublishFolder(directory, appRoot);
			}
		}
	}
}