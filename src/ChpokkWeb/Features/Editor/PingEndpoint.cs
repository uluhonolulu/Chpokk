using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Ajax;

namespace ChpokkWeb.Features.Editor {
	public class PingEndpoint {
		//[AuthorizedBy(typeof(MyAuthorizationPolicy))]
		public AjaxContinuation KeepAlive() {
			return AjaxContinuation.Successful();
		}
	}
}