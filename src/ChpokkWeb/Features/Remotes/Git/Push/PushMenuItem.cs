using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Remotes.Git.Push {
	public class PushMenuItem: MenuItem {
		public PushMenuItem() {
			Id = "pusher";
			Caption = "Push";
		}
	}
}