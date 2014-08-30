using System.Collections.Generic;
using System.Threading;

namespace ChpokkWeb.Features.Storage {
	public class RestoreSynchronizer {
		//public ManualResetEvent _resetEvent = new ManualResetEvent(false);
		IDictionary<string, ManualResetEvent> _resetEvents = new Dictionary<string, ManualResetEvent>();
		public void RestoringStarted(string path) {
			EnsureEvent(path);
			_resetEvents[path].Reset();
		}

		public void RestoringFinished(string path) {
			EnsureEvent(path);
			_resetEvents[path].Set();
		}

		public void WaitTillRestored(string path) {
			//If we have set the event before, let's wait for it. If not, do nothing.
			if(ShouldWaitFor(path))
				_resetEvents[path].WaitOne();
		}

		private void EnsureEvent(string path ) {
			if (!_resetEvents.ContainsKey(path)) {
				_resetEvents[path] = new ManualResetEvent(false);
			}
		}

		private bool ShouldWaitFor(string path) {
			return _resetEvents.ContainsKey(path);
		}
	}
}