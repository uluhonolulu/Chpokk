using System.Collections.Generic;

namespace Chpokk.Tests.Git {
	public class RemoteListModel {
		public IEnumerable<string> Remotes { get; set; }
		public string DefaultRemote { get; set; }
	}
}