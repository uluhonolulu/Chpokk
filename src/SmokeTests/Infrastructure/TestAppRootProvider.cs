using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace Chpokk.Tests.Infrastructure {
	class TestAppRootProvider: IAppRootProvider {
		public string AppRoot {
			get {
				if (HostingEnvironment.IsHosted) {
					return HostingEnvironment.ApplicationPhysicalPath;
				}
				return AppDomain.CurrentDomain.BaseDirectory.ParentDirectory();
			}
		}
	}
}
