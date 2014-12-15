using System;
using System.IO;
using Emkay.S3;
using FubuCore;

namespace ChpokkWeb.Features.Storage {
	public class Uploader {
		private readonly IS3Client _client;
		public Uploader(IS3Client client) {
			_client = client;
		}


		public void UploadFolder(string path, string appRoot) {
			foreach (var filePath in Directory.GetFiles(path)) {
				var pathRelativetoAppRoot = filePath.PathRelativeTo(appRoot);
				var key = pathRelativetoAppRoot.Replace('\\', '/');
				_client.PutFile("chpokk", key, filePath, true, Int32.MaxValue);
			}

			foreach (var directory in Directory.GetDirectories(path)) {
				UploadFolder(directory, appRoot);
			}
		}
	}
}