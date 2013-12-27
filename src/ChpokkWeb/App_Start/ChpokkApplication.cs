using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using StructureMap;

namespace ChpokkWeb.App_Start {
	public class ChpokkApplication : IApplicationSource{
		public FubuApplication BuildApplication() {
        	var container = new Container();
        	container.Configure(expression => expression.AddRegistry<ChpokkRegistry>());
			return FubuApplication.For<ConfigureFubuMVC>().StructureMap(container);
		}
	}
}