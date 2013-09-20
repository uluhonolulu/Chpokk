using ChpokkWeb.Infrastructure;
using FubuMVC.Core;

namespace ChpokkWeb.Features.Exploring {
	[UrlPattern("/Repository/{RepositoryName}")]
	public class RepositoryInputModel : BaseRepositoryInputModel {
	}

	public class FileListInputModel : RepositoryInputModel {
	}
}