using FubuMVC.Core;

namespace ChpokkWeb.Features.Repository {
	public class RepositoryController {
		[UrlPattern("Repository/{Name}")]
		public RepositoryModel Get(RepositoryInputModel input) {
			return new RepositoryModel(){Name = input.Name};
		}
	}
}