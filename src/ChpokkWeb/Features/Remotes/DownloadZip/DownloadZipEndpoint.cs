using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using ChpokkWeb.Infrastructure.SimpleZip;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using ICSharpCode.SharpZipLib.Zip;
using FubuCore;

namespace ChpokkWeb.Features.Remotes.DownloadZip {
	public class DownloadZipEndpoint {
		private readonly IHttpWriter _writer;
		private readonly RepositoryManager _manager;
		private readonly Zipper _zipper;

		public DownloadZipEndpoint(IHttpWriter writer, HttpContextBase httpContextBase, RepositoryManager manager, Zipper zipper) {
			_writer = writer;
			_manager = manager;
			_zipper = zipper;
		}

		public void Download(DownloadZipInputModel inputModel) {
			_writer.WriteContentType("application/zip");
			_writer.AppendHeader("content-disposition", "attachment; filename=\"{0}\"".ToFormat(inputModel.RepositoryName));
			var folderName = _manager.GetAbsolutePathFor(inputModel.RepositoryName, inputModel.PhysicalApplicationPath);
			_writer.Write(stream => _zipper.DownloadZippedFolder(folderName, stream));
		}
	}
}