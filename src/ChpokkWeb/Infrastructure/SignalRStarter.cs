using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(SignalRStarter))]
namespace ChpokkWeb.Infrastructure {
	public class SignalRStarter {
		public void Configuration(IAppBuilder app) {
			// Any connection or hub wire up and configuration should go here
			app.MapSignalR();
		}
	}
}