using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;

namespace ChpokkWeb.Features.Authentication {
	[ConfigurationType(ConfigurationType.Instrumentation)]
	public class SignoutJohnDoeBehavior: IActionBehavior {
		private readonly IActionBehavior _inner;
		private readonly UserManagerInContext _userManager;
		public SignoutJohnDoeBehavior(IActionBehavior inner, UserManagerInContext userManager) {
			_inner = inner;
			_userManager = userManager;
		}

		public void Invoke() {
			_userManager.ForceUnregisteredUserToSignout();
			_inner.Invoke();
		}
		public void InvokePartial() {
			_inner.InvokePartial();
		}
	}
}