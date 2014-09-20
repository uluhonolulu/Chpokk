using System;
using System.IO;
using System.Web.Hosting;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Assets.Tags;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Packaging;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;
using FubuMVC.Core.Urls;
using FubuMVC.StructureMap;
using Gallio.Runtime.Extensibility.Schema;
using StructureMap;
using FubuCore;
using System.Linq;

namespace UnitTests.Infrastructure {
	public class SimpleConfiguredContext : SimpleContext, IDisposable {
		public override void Create() {
			AssetDeclarationVerificationActivator.Latched = false; // skip asset checking
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
			registry.Assets().HandleMissingAssetsWith<TraceOnlyMissingAssetHandler>(); //ignore missing assets for now
		}

		private static void ConfigureContainer(Container container) {
			container.Configure(expression => expression.AddRegistry<ChpokkRegistry>());
			container.Configure(expr => expr.For<IUrlRegistry>().Use<UrlRegistry>());
			container.Configure(expression => expression.For<IAppRootProvider>().Use<TestAppRootProvider>());
			container.Configure(expression => expression.For<IActionBehavior>().Use<NulloBehavior>());
			//Console.WriteLine(container.WhatDoIHave() );
			
		}

		[NotNull]
		private IServiceFactory _serviceFactory;

		private static readonly Container _container;

		static SimpleConfiguredContext() {
			//AssemblyLocator.Init(); //fix the missing assembly error
			//var _ = new FubuMVC.Validation.ValidationMode("-");//fix the missing assembly error
			//Console.WriteLine("FubuMvcPackageFacility.PhysicalRootPath: " + FubuMvcPackageFacility.PhysicalRootPath);
			//Console.WriteLine("HostingEnvironment.ApplicationPhysicalPath: " + HostingEnvironment.ApplicationPhysicalPath);
			string str = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
			Console.WriteLine("Assembly loaded from: " + typeof(SimpleConfiguredContext).Assembly.Location);
			str = typeof (SimpleConfiguredContext).Assembly.Location.ParentDirectory();
			Console.WriteLine("Assembly folder: " + str);
			if (str.EndsWith("bin"))
				str = str.Substring(0, str.Length - 3).TrimEnd(Path.DirectorySeparatorChar);
			Console.WriteLine("FubuMvcPackageFacility.determineApplicationPathFromAppDomain(): " + str);
			var contentFolder = Directory.GetDirectories(str, "*Content").FirstOrDefault();
			if (contentFolder != null) {
				Console.WriteLine("Found content at " + contentFolder);
			}
			Console.WriteLine("Current folder: " + Directory.GetCurrentDirectory());
			FubuMvcPackageFacility.PhysicalRootPath = str;

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
