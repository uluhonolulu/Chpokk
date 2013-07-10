using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Storage {
	public class RestoreEndpoint {
		private readonly Downloader _downloader;
		public RestoreEndpoint(Downloader downloader) {
			_downloader = downloader;
		}

		public string Restore(RestoreInputModel input) {
			var builder = new StringBuilder();
			_downloader.DownloadAllFiles(input.PhysicalApplicationPath, s => builder.AppendLine(s));
			return builder.ToString();
		}
	}
	public class RestoreInputModel {
		[NotNull]
		public string PhysicalApplicationPath { get; set; }		
	}
}