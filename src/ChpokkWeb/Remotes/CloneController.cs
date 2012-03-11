using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core;
using FubuMVC.Core.Ajax;

namespace ChpokkWeb.Remotes {
	public class CloneController {
		[JsonEndpoint]
		public AjaxContinuation Test() {
			return new AjaxContinuation(){ShouldRefresh = true};
		}

		[JsonEndpoint]
		public AjaxContinuation CloneRepo(CloneInputModel model) {
			return new AjaxContinuation(){ShouldRefresh = true};
		}
	}

	public class CloneInputModel {
		public string RepoUrl { get; set; }
		public string PhysicalApplicationPath { get; set; }
	}
}