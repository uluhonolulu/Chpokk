using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Infrastructure {
	public class AspNetAppRootProvider: IAppRootProvider {
		private readonly HttpContextBase _context;
		public AspNetAppRootProvider(HttpContextBase context) {
			_context = context;
		}

		public string AppRoot {
			get { return _context.Server.MapPath("/"); }
		}
	}
}