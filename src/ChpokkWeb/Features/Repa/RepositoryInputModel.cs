using ChpokkWeb.Shared;

namespace ChpokkWeb.Features.Repa {
	public class RepositoryInputModel : IDontNeedActionsModel {
		public string Name { get; set; }
	}

	public class FileListModel : RepositoryInputModel {
		public string PhysicalApplicationPath { get; set; }		
	}
}