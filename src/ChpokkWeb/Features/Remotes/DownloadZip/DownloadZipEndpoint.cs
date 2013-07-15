using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using ICSharpCode.SharpZipLib.Zip;
using FubuCore;

namespace ChpokkWeb.Features.Remotes.DownloadZip {
	public class DownloadZipEndpoint {
		private readonly IHttpWriter _writer;
		private RepositoryManager _manager;

		public DownloadZipEndpoint(IHttpWriter writer, HttpContextBase httpContextBase, RepositoryManager manager) {
			_writer = writer;
			_manager = manager;
		}

		public void Download(DownloadZipInputModel inputModel) {
			_writer.WriteContentType("application/zip");
			_writer.AppendHeader("content-disposition", "attachment; filename=\"{0}\"".ToFormat(inputModel.RepositoryName));
			var folderName = _manager.GetAbsolutePathFor(inputModel.RepositoryName, inputModel.PhysicalApplicationPath);
			_writer.Write(stream => DownloadZipedFolder(folderName, stream));
		}

		private void DownloadZipedFolder(string folderName	, Stream responseStream) {
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