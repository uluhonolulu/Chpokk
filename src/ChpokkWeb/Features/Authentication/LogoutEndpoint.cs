using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Demo;
using FubuMVC.Core.Ajax;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Continuations;

namespace ChpokkWeb.Features.Authentication {
	public class LogoutEndpoint {
		private readonly UserManager _userManager;
		public LogoutEndpoint(UserManager userManager) {
			_userManager = userManager;
		}

		public FubuContinuation DoIt(LogoutDummyInputModel _) {
			_userManager.SignoutUser();
			return  FubuContinuation.RedirectTo<DemoModel>();
		}
	}

	public class LogoutDummyInputModel {}
}