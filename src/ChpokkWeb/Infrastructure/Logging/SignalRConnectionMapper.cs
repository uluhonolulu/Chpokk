using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Infrastructure.Logging {
	public class SignalRConnectionMapper {
		readonly IDictionary<string, string> _connections = new Dictionary<string, string>();

		public IDictionary<string, string> Connections
		{
			get { return _connections; }
		}
	}
}