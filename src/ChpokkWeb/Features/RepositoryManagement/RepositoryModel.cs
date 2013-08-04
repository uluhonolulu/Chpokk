using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryModel: BaseRepositoryInputModel {
		public MenuItem[] RetrieveActions { get; set; }
	}
}