using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Ajax;

namespace ChpokkWeb.Infrastructure {
	public class KeepAliveEndpoint {
		public AjaxContinuation KeepAlive() {
			return AjaxContinuation.Successful();
		}
	}
}