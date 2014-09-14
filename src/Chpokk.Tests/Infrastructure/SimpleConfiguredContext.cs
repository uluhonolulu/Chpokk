using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arractas;
using ChpokkWeb;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;
using FubuMVC.Core.Urls;
using FubuMVC.StructureMap;
using NuGet.Common;
using StructureMap;

namespace Chpokk.Tests.Infrastructure {
	public class SimpleConfiguredContext : SimpleContext, IDisposable {
		public override void Create() {
			var childContainer = _container.GetNestedContainer();
			_serviceFactory = CreateFacility(childContainer).BuildFactory();
		}

		private IContainerFacility CreateFacility(IContainer childContainer) {
			var registry = new ConfigureFubuMVC();
			ConfigureFubuRegistry(registry);
			var runtime = FubuApplication.For(registry)
				.StructureMap(childContainer)
				.Bootstrap()
				;
			return runtime.Facility;
		}

		protected virtual void ConfigureFubuRegistry(ConfigureFubuMVC registry) {
			UseFakeSecurityContext(registry);
		}

		private static void ConfigureContainer(Container container) {
			container.Configure(expression =>
			{
				expression.AddRegistry<ChpokkRegistry>();
			});
			container.Configure(expression =>
			{
				expression.For<IUrlRegistry>().Use<UrlRegistry>();
				expression.For<IAppRootProvider>().Use<TestAppRootProvider>();
				expression.For<IActionBehavior>().Use<NulloBehavior>();
				expression.For<IConsole>().Use<NuGet2Console>();
			});
			//Console.WriteLine(container.WhatDoIHave() );
			
		}

		[NotNull]
		private IServiceFactory _serviceFactory;

		private static readonly Container _container;

		static SimpleConfiguredContext() {
			_container = new Container();
			ConfigureContainer(_container);
		}

		public IServiceFactory Container { get { return _serviceFactory; } }

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

		public virtual void Dispose() {
			
		}
	}
}
