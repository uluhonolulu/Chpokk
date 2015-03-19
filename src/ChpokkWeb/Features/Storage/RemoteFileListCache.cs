using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Emkay.S3;

namespace ChpokkWeb.Features.Storage {
	public class RemoteFileListCache {
		private readonly IS3Client _client;
		private readonly Lazy<IEnumerable<string>> _paths;
		public RemoteFileListCache(IS3Client client) {
			_client = client;
			_paths = new Lazy<IEnumerable<string>>(GetRemoteFilePaths);
		}

		public IEnumerable<string> Paths {
			get { return _paths.Value; }
		}

		private IEnumerable<string> GetRemoteFilePaths() {
			return _client.EnumerateChildren("chpokk");
		}


	}
}