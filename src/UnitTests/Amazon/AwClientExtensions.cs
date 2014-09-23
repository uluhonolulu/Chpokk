using System;
using System.Linq;
using Emkay.S3;

namespace UnitTests.Amazon {
	public static class AwClientExtensions {
		public static bool Exists(this IS3Client client, string path) {
			var allFiles = client.EnumerateChildren("chpokk");
			return allFiles.Contains(path, StringComparer.InvariantCultureIgnoreCase);
		}
	}
}
