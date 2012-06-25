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

		public static AjaxContinuation ForException([NotNull] this AjaxContinuation continuation, [NotNull] Exception exception) {
			continuation.Errors.Add(new AjaxError(){category = exception.Message,  message = exception.ToString()});
			return continuation;
		}
	}
}