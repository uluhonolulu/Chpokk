using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Remotes.Git.Push {
	public class PushInputModel: BaseRepositoryInputModel {

		public string Remote { get; set; }
		public string NewRemote { get; set; }
		public string NewRemoteUrl { get; set; }

		public string Username { get; set; }
		public string Password { get; set; }

		public string ConnectionId { get; set; }
	}
}