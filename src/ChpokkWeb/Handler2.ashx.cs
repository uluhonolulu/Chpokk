using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb {
	/// <summary>
	/// Summary description for Handler2
	/// </summary>
	public class Handler2 : IHttpHandler {

		public void ProcessRequest(HttpContext context) {
			context.Response.ContentType = "text/plain";
			if (context.User != null) {
				context.Response.Write(context.User.Identity.Name);
			}
			else {
				context.Response.Write("anonymous");
			}
			
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}