using ChpokkWeb.Shared;
using FubuMVC.Core;

namespace ChpokkWeb.Features.Exploring {
	[UrlPattern("/Repository/{Name}")]
	public class RepositoryInputModel : IDontNeedActionsModel {
		public string Name { get; set; }
	}

	public class FileListInputModel : RepositoryInputModel {
		public string PhysicalApplicationPath { get; set; }		
	}
}