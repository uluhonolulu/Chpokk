using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryModel {
		public string RepositoryName { get; set; }
		public MenuItem[] RetrieveActions { get; set; }
	}
}