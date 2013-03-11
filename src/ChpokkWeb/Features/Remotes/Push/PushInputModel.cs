using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Remotes.Push {
	public class PushInputModel {
		[NotNull]
		public string RepositoryName { get; set; }
		[NotNull]
		public string PhysicalApplicationPath { get; set; }

		public string Username { get; set; }
		public string Password { get; set; }
	}
}