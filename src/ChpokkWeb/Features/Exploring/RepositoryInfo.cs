using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Exploring {
	public class RepositoryInfo {
		public RepositoryInfo([NotNull] string path, [NotNull] string name) {
			Name = name;
			Path = path;
		}

		[NotNull] 
		public string Path { get; private set; }

		[NotNull] 
		public string Name { get; private set; }
	}
}