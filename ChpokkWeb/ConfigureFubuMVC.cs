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

namespace ChpokkWeb
{
    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC()
        {
            // This line turns on the basic diagnostics and request tracing
            IncludeDiagnostics(true);

            // All public methods from concrete classes ending in "Controller"
            // in this assembly are assumed to be action methods
            Actions.IncludeClassesSuffixedWithController();

            // Policies
            Routes
                .IgnoreControllerNamesEntirely()
                //.IgnoreMethodSuffix("Html")
                .RootAtAssemblyNamespace();

			//Output.ToJson.WhenCallMatches(action => action.HasAttribute<JsonEndpointAttribute>());

			((FubuRegistry)this).UseSpark();

            // Match views to action methods by matching
            // on model type, view name, and namespace
			//ITemplateRegistry registry = null;
			//this.Services(reg =>
			//                {
			//                    registry = reg.DefaultServiceFor<ITemplateRegistry>().Value as ITemplateRegistry;
			//                });
			Views
				//.TryToAttachWithDefaultConventions()
				.RegisterActionLessViews(
				token => {
					return token.ViewModelType == typeof(DummyModel);
				}, chain =>
				   	{
				   		chain.Route = new RouteDefinition("demostuff");
				   	})
					//.Facility(new ModellessSparkViewFacility(registry))
				;
        }
    }

	public class ModellessSparkViewFacility : IViewFacility {
		private readonly ITemplateRegistry _templateRegistry;
		public ModellessSparkViewFacility(ITemplateRegistry templateRegistry) {
			_templateRegistry = templateRegistry;
		}

		public IEnumerable<IViewToken> FindViews(TypePool types, BehaviorGraph graph) {
			var descriptors = from template in _templateRegistry.AllTemplates()
			                  where template.Descriptor is ViewDescriptor
			                  select template.Descriptor as ViewDescriptor;
			var tokens = from descriptor in descriptors where !descriptor.HasViewModel() select new SparkViewToken(descriptor);
			return tokens.Cast<IViewToken>();

		}
	}
}