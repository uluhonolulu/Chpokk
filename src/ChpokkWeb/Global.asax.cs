﻿using System;
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
using ChpokkWeb.Features.Storage;
using FubuMVC.Core;
using FubuMVC.Core.Security;
using StructureMap;
using StructureMap.Pipeline;

namespace ChpokkWeb {
	public class Global : System.Web.HttpApplication {
		private FubuRuntime _fubuRuntime;

		protected void Application_Start(object sender, EventArgs e) {
			_fubuRuntime = AppStartFubuMVC.Start();
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
			var key = HttpContextLifecycle.ITEM_NAME;
			var cache = this.Session[key] as IObjectCache;
			if (cache != null) {
				cache.DisposeAndClear();
			}
		}

		protected void Application_End(object sender, EventArgs e) {

		}
	}
}