using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using StructureMap;

[assembly: OwinStartup(typeof(SignalRStarter))]
namespace ChpokkWeb.Infrastructure {
	public class SignalRStarter {
		public void Configuration(IAppBuilder app) {
			// Any connection or hub wire up and configuration should go here
			GlobalHost.DependencyResolver = ObjectFactory.GetInstance<IDependencyResolver>();
			app.MapSignalR();
		}
	}
}