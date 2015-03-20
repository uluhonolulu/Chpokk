using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;

namespace ChpokkWeb.Features.Storage {
	public class RestoreEndpoint {
		private readonly Downloader _downloader;
		private readonly RemoteFileListCache _cache;
		public RestoreEndpoint(Downloader downloader, RemoteFileListCache cache) {
			_downloader = downloader;
			_cache = cache;
		}

		public string Restore(RestoreInputModel input) {
			var builder = new StringBuilder();
			_downloader.DownloadAllFiles(input.PhysicalApplicationPath, null, (s, l) => builder.AppendLine(s));
			return builder.ToString();
		}

		[JsonEndpoint]
		public object AllFiles() {
			return _cache.Paths.Count();
		}  
	}
	public class RestoreInputModel {
		[NotNull]
		public string PhysicalApplicationPath { get; set; }		
	}
}