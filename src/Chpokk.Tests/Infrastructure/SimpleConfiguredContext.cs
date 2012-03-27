using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arractas;
using ChpokkWeb;
using FubuMVC.Core;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Urls;
using FubuMVC.StructureMap;
using StructureMap;

namespace Chpokk.Tests.Infrastructure {
	public class SimpleConfiguredContext : SimpleContext{
		public override void Create() {
			//base.Create();
			SetupContainer();
		}
		private IContainerFacility SetupContainer() {
			var container = new Container();
			//container.Configure(expr => expr.For<IUrlRegistry>().Use<UrlRegistry>());
			var runtime = FubuApplication.For<ConfigureFubuMVC>()
				.StructureMap(container)
				.Bootstrap()
				;
			return runtime.Facility;
			//_container.Inject(typeof(IUrlRegistry), typeof(UrlRegistry));
		}

		private IContainerFacility _container;
		public IContainerFacility Container {
			get {
				if(_container == null)
					_container = SetupContainer();
				return _container;
			}
		}

	}
}
