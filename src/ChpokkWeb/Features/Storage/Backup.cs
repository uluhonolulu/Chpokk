using System.IO;
using ChpokkWeb.Features.Exploring;
using FubuMVC.Core;

namespace ChpokkWeb.Features.Storage {
	public class Backup {
		private readonly Uploader _uploader;
		public Backup(Uploader uploader, ApplicationSettings settings) {
			_uploader = uploader;
			AppRoot = settings.GetApplicationFolder();
		}

		private string AppRoot { get; set; }
		public void PublishRepository(RepositoryInfo repositoryInfo) {
			_uploader.PublishFolder(Path.Combine(AppRoot, repositoryInfo.Path), AppRoot);
		}
	}
}