using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ChpokkWeb {
	/// <summary>
	/// Summary description for Handler1
	/// </summary>
	public class Handler1 : IHttpHandler {

		public void ProcessRequest(HttpContext context) {
			FormsAuthentication.SetAuthCookie("ulu", true);
			context.Response.Redirect("Handler2.ashx");
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}