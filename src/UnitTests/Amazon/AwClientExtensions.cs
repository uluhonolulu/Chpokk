using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emkay.S3;

namespace Chpokk.Tests.Amazon {
	public static class AwClientExtensions {
		public static bool Exists(this IS3Client client, string path) {
			var allFiles = client.EnumerateChildren("chpokk");
			return allFiles.Contains(path, StringComparer.InvariantCultureIgnoreCase);
		}
	}
}
