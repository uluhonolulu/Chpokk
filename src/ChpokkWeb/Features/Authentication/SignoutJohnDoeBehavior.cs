using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.MainScreen;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;

namespace ChpokkWeb.Features.Authentication {
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

	[ConfigurationType(ConfigurationType.InjectNodes)]
	public class SignoutJohnDoeConfiguration :  IConfigurationAction {
		public void Configure(BehaviorGraph graph) {
			var chain = graph.BehaviorFor(typeof(MainDummyModel));
			chain.InsertFirst(Wrapper.For<SignoutJohnDoeBehavior>());
		}
	}
}