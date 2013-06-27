using System.Security.Principal;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryListInputModel {
		public string PhysicalApplicationPath { get; set; }
		public IPrincipal User  { get; set; }
	}
}