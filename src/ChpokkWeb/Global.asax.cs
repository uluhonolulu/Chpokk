using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ChpokkWeb.App_Start;
using ChpokkWeb.Features.Editor.Compilation;
using ChpokkWeb.Features.Editor.Intellisense;
using ChpokkWeb.Features.ProjectManagement;
using StructureMap;

namespace ChpokkWeb {
	public class Global : System.Web.HttpApplication {

		protected void Application_Start(object sender, EventArgs e) { 
			AppStartFubuMVC.Start(); 
			ObjectFactory.Configure(
				expr => { expr.For<SmtpClient>().Use(() => new SmtpClient()); expr.SelectConstructor(() => new SmtpClient());}
				);

			//warmup
			ProjectData.WarmUp();
		}

		protected void Session_Start(object sender, EventArgs e) {

		}

		protected void Application_BeginRequest(object sender, EventArgs e) {

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e) {

		}

		protected void Application_Error(object sender, EventArgs e) {

		}

		protected void Session_End(object sender, EventArgs e) {

		}

		protected void Application_End(object sender, EventArgs e) {

		}
	}
}