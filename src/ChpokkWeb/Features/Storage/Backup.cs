using System.IO;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Storage {
	public class Backup {
		private readonly Uploader _uploader;
		private readonly IAppRootProvider _rootProvider;
		public Backup(Uploader uploader, IAppRootProvider rootProvider) {
			_uploader = uploader;
			_rootProvider = rootProvider;
		}

		public void PublishRepository(RepositoryInfo repositoryInfo) {
			_uploader.PublishFolder(Path.Combine(_rootProvider.AppRoot, repositoryInfo.Path), _rootProvider.AppRoot);
		}
	}
}