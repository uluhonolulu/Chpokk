using System.Collections.Generic;

namespace ChpokkWeb.Features.Remotes.Git.Remotes {
	public class RemoteListModel {
		public IEnumerable<string> Remotes { get; set; }
		public string DefaultRemote { get; set; }
	}
}