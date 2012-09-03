using ChpokkWeb.Features.Project;
using FubuMVC.Core;

namespace ChpokkWeb.Features.Repository {
	public class RepositoryController {
		[UrlPattern("Repository/{Name}")]
		public ProjectModel Get(ProjectInputModel input) {
			return new ProjectModel(){Name = input.Name};
		}
	}
}