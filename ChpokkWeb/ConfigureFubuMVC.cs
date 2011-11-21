using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ChpokkWeb.Shared;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.View;
using FubuMVC.Spark;
using FubuMVC.Spark.SparkModel;
using StructureMap;

namespace ChpokkWeb {
	public class ConfigureFubuMVC : FubuRegistry {
		public ConfigureFubuMVC() {
			// This line turns on the basic diagnostics and request tracing
			IncludeDiagnostics(true);

			// All public methods from concrete classes ending in "Controller"
			// in this assembly are assumed to be action methods
			Actions.IncludeClassesSuffixedWithController();

			// Policies
			Routes
				.IgnoreControllerNamesEntirely()
				.RootAtAssemblyNamespace()
				;

			this.UseSpark();

			Views
				.TryToAttachWithDefaultConventions()
				.RegisterActionLessViews(
				token => token.ViewModelType == typeof(DummyModel), (chain, token) =>
				        {
				            var url = (token.Name == "DemoView") ? "" : token.Name;
				            chain.Route = new RouteDefinition(url);
				        })
				;
		}
	}


}