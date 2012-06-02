using ChpokkWeb.Shared;

namespace ChpokkWeb.Features.Exploring {
	public class RepositoryInputModel : IDontNeedActionsModel {
		public string Name { get; set; }
	}

	public class FileListInputModel : RepositoryInputModel {
		public string PhysicalApplicationPath { get; set; }		
	}
}