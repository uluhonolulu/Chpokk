namespace ChpokkWeb.Features.Exploring {
	public class RepositoryInfo {
		public RepositoryInfo(string path, string name) {
			Name = name;
			Path = path;
		}

		public string Path { get; private set; }

		public string Name { get; private set; }
	}
}