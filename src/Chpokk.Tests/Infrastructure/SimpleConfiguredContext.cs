using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arractas;
using ChpokkWeb;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;
using FubuMVC.Core.Urls;
using FubuMVC.StructureMap;
using StructureMap;

namespace Chpokk.Tests.Infrastructure {
	public class SimpleConfiguredContext : SimpleContext{
		public override void Create() {
			_container = new Lazy<IServiceFactory>(() => CreateFacility().BuildFactory());
		}

		private IContainerFacility CreateFacility() {
			var container = new Container();
			ConfigureContainer(container);
			var registry = new ConfigureFubuMVC();
			ConfigureFubuRegistry(registry);
			var runtime = FubuApplication.For(registry)
				.StructureMap(container)
				.Bootstrap()
				;
			return runtime.Facility;
		}

		protected virtual void ConfigureFubuRegistry(ConfigureFubuMVC registry) {
			UseFakeSecurityContext(registry);
		}

		protected virtual void ConfigureContainer(Container container) {
			container.Configure(expression => expression.AddRegistry<ChpokkRegistry>());
			container.Configure(expr => expr.For<IUrlRegistry>().Use<UrlRegistry>());
		}


		private Lazy<IServiceFactory> _container;
		public IServiceFactory Container { get { return _container.Value; } }

		public string AppRoot {
			get { return Path.GetFullPath(@".."); }
		}

		public FakeSecurityContext FakeSecurityContext { get; set; }

		public void UseFakeSecurityContext(FubuRegistry registry) {
			if (FakeSecurityContext == null) {
				FakeSecurityContext = new FakeSecurityContext();
			}
			registry.Services(serviceRegistry => serviceRegistry.ReplaceService<ISecurityContext>(FakeSecurityContext));
		}
	}
}
