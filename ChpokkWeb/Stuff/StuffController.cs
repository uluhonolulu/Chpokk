using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Stuff {
	public class StuffController {
		public StuffModel Index(InputStuffModel input) {
			return new StuffModel();
		}

		public string Version() {
			return Environment.OSVersion.ToString();
		}

		public OtherModel WebForms() {
			return new OtherModel();
		}
	}
}