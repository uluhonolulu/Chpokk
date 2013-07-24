using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FubuCore;
using ICSharpCode.SharpZipLib.Zip;

namespace ChpokkWeb.Infrastructure.SimpleZip {
	public class Zipper {
		private readonly IFileSystem _fileSystem;
		public Zipper(IFileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public void UnzipStream(string repositoryPath, Stream fileStream) {
			using (var zipFile = new ZipFile(fileStream) {IsStreamOwner = true}) {
				foreach (ZipEntry zipEntry in zipFile) {
					if (!zipEntry.IsDirectory) {
						var fileName = repositoryPath.AppendPath(zipEntry.Name);
						_fileSystem.WriteStreamToFile(fileName, zipFile.GetInputStream(zipEntry));
					}
				}
			}
		}

		public void DownloadZippedFolder(string folderName	, Stream responseStream) {
			using (var zipOutputStream = new ZipOutputStream(responseStream)) {
				foreach (var fileName in Directory.GetFiles(folderName, "*.*", SearchOption.AllDirectories)) {
					ZipFile(fileName, folderName, zipOutputStream);
				} 
			}
		}

		private void ZipFile(string fileName, string root, ZipOutputStream zipOutputStream) {
			var buffer = new byte[4096];
			using (Stream fs = File.OpenRead(fileName)) {
				var entry = new ZipEntry(ZipEntry.CleanName(fileName.PathRelativeTo(root))) {Size = fs.Length};
				zipOutputStream.PutNextEntry(entry);
				var count = fs.Read(buffer, 0, buffer.Length);
				while (count > 0) {
					zipOutputStream.Write(buffer, 0, count);
					count = fs.Read(buffer, 0, buffer.Length);
				}
			}
		}
	}
}