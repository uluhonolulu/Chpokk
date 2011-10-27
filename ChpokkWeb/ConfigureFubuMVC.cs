using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ChpokkWeb.Shared;
using ChpokkWeb.Stuff;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.View;
using FubuMVC.Spark;
using FubuMVC.Spark.SparkModel;
using FubuMVC.WebForms;
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
				//.HomeIs<DummyModel>()
				.HomeIs<InputStuffModel>()
				;

			this.UseSpark();
			this.Import<WebFormsEngine>();

			Views
				.TryToAttachWithDefaultConventions()
				//.RegisterActionLessViews(
				//token => token.ViewModelType == typeof(DummyModel), chain => {
				//    chain.Route = new RouteDefinition("demo");
				//})
				;
		}
	}


}