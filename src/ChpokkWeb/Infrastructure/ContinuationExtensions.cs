using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Ajax;

namespace ChpokkWeb.Infrastructure {
	public static class ContinuationExtensions {
		public static AjaxContinuation NavigateTo(this AjaxContinuation continuation, string url) {
			continuation["navigatePage"] = url;
			return continuation;
		}

		public static AjaxContinuation ForException(this AjaxContinuation continuation, Exception exception) {
			continuation.Errors.Add(new AjaxError(){message = exception.Message});
			return continuation;
		}
	}
}