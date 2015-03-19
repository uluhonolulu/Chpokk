using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Storage {
	public class RestoreEndpoint {
		private readonly Downloader _downloader;
		private RemoteFileListCache _cache;
		public RestoreEndpoint(Downloader downloader, RemoteFileListCache cache) {
			_downloader = downloader;
			_cache = cache;
		}

		public string Restore(RestoreInputModel input) {
			var builder = new StringBuilder();
			_downloader.DownloadAllFiles(input.PhysicalApplicationPath, null, (s, l) => builder.AppendLine(s));
			return builder.ToString();
		}

		public IEnumerable<string> AllFiles() {
			return _cache.Paths;
		}  
	}
	public class RestoreInputModel {
		[NotNull]
		public string PhysicalApplicationPath { get; set; }		
	}
}