using System.IO;

namespace ChpokkWeb.Infrastructure {
	public static class PathExtensions {
		public static string AppendPathMyWay(this string path, params string[] appended) {
			foreach (var next in appended) {
				var toAppend = next;
				if (Path.IsPathRooted(next)) {
					toAppend = next.Substring(Path.GetPathRoot(next).Length);
				}
				path = Path.Combine(path, toAppend?? string.Empty);
			}
			return path;
		}
	}
}
