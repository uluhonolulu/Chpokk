using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryModel: BaseRepositoryModel {
		public MenuItem[] RetrieveActions { get; set; }
	}
}