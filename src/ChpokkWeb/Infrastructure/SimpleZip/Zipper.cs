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
	}
}