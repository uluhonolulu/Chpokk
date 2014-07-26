using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ChpokkWeb.Infrastructure {
	public class AspNetAppRootProvider: IAppRootProvider {
		public string AppRoot {
			get { return HostingEnvironment.ApplicationPhysicalPath; }
		}
	}
}