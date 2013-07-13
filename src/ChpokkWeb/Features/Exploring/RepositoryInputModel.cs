using ChpokkWeb.Infrastructure;
using FubuMVC.Core;

namespace ChpokkWeb.Features.Exploring {
	[UrlPattern("/Repository/{RepositoryName}")]
	public class RepositoryInputModel : IDontNeedActionsModel {
		public string Name { get; set; }
	}

	public class FileListInputModel : RepositoryInputModel {
		public string PhysicalApplicationPath { get; set; }		
	}
}