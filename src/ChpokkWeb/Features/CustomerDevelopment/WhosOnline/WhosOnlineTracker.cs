using System.Collections.Generic;

namespace ChpokkWeb.Features.CustomerDevelopment.WhosOnline {
	public class WhosOnlineTracker {
		readonly IList<string> _users = new List<string>();

		public void On(string userName) {
			if (!_users.Contains(userName)) {
				_users.Add(userName);
			}
		}

		public void Off(string userName) {
			_users.Remove(userName);
		}

		public IList<string> Who { get { return _users; } }
	}
}