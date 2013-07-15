using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using ICSharpCode.SharpZipLib.Zip;

namespace ChpokkWeb.Features.Remotes.DownloadZip {
	public class DownloadZipEndpoint {
		private IHttpWriter _writer;
		private HttpContextBase _httpContextBase;
		public DownloadZipEndpoint(IHttpWriter writer, HttpContextBase httpContextBase) {
			_writer = writer;
			_httpContextBase = httpContextBase;
		}

		public void Download(DownloadZipModel model) {
			//return new DownloadDataModel() { MimeType = "text/plain", Filename = "x.txt"};
			_writer.WriteContentType("application/zip");
			_writer.AppendHeader("content-disposition", "attachment; filename=\"Download.zip\"");
			_writer.Write(stream => DownloadZipToBrowser(@"C:\log.txt", stream));
		}

		private void DownloadZipToBrowser(string fileName, Stream responseStream) {


			var buffer = new byte[4096];
			var zipOutputStream = new ZipOutputStream(responseStream);
			zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression

			using (Stream fs = File.OpenRead(fileName)) {
				var entry = new ZipEntry(ZipEntry.CleanName(fileName)) {Size = fs.Length};
				zipOutputStream.PutNextEntry(entry);
				var count = fs.Read(buffer, 0, buffer.Length);
				while (count > 0) {
					zipOutputStream.Write(buffer, 0, count);
					count = fs.Read(buffer, 0, buffer.Length);
				}
			}

			zipOutputStream.Close();

		}
	}

	public class DownloadZipModel {}
}