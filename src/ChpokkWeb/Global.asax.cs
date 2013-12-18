using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ChpokkWeb.App_Start;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.Editor.Intellisense;
using ChpokkWeb.Features.ProjectManagement;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ChpokkWeb.Features.Storage;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;
using Microsoft.AspNet.SignalR;
using StructureMap;
using StructureMap.Pipeline;

namespace ChpokkWeb {
	public class Global : System.Web.HttpApplication {
		private FubuRuntime _fubuRuntime;

		protected void Application_Start(object sender, EventArgs e) {
			_fubuRuntime = AppStartFubuMVC.Start();

			RegisterFonts();

			RegisterSignalResolver();


			//restore all files
			//_fubuRuntime.Factory.Get<Restore>().RestoreAll();

			//prewarm NuGet
			//_fubuRuntime.Factory.Get<Prewarmer>().PrewarmAsync();
		}

		private void RegisterSignalResolver() {
			GlobalHost.DependencyResolver = _fubuRuntime.Factory.Get<StructureMapResolver>();
		}

		protected void Session_Start(object sender, EventArgs e) {

		}

		protected void Application_BeginRequest(object sender, EventArgs e) {
		}

		//protected void Application_AuthenticateRequest(object sender, EventArgs e) {
		//	// if the user is authenticated, but not in the database, force it to log out so that it signs in via Janrain
		//	_fubuRuntime.Factory.Get<UserManagerInContext>().ForceUnregisteredUserToSignout();

		//}

		protected void Application_Error(object sender, EventArgs e) {

		}

		protected void Session_End(object sender, EventArgs e) {
			var key = HttpContextLifecycle.ITEM_NAME;
			var cache = this.Session[key] as IObjectCache;
			if (cache != null) {
				cache.DisposeAndClear();
			}
		}

		protected void Application_End(object sender, EventArgs e) {

		}

		public static void RegisterFonts() {
			MimeType.New("application/font-woff", ".woff");
			MimeType.New("application/vnd.ms-fontobject", ".eot");
			MimeType.New("image/svg+xml", ".svg");
			
		}
	}
}