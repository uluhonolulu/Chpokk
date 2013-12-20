using System.Web.Routing;
using Bottles;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using StructureMap.Configuration.DSL;

namespace $rootnamespace$
{
    // Using a separate class for bootstrapping makes it much easier to reuse your application 
    // in testing scenarios with either SelfHost or OWIN/Katana hosting
    public class MyApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            // This is bootstrapping an application with all default FubuMVC conventions and
            // policies pulling actions from only this assembly for classes suffixed with
            // "Endpoint" or "Endpoints"
            return FubuApplication.DefaultPolicies().StructureMap<MyStructureMapRegistry>();



            // Fancier way if you want to specify your own policies:
            // return FubuApplication.For<MyFubuMvcPolicies>().StructureMap(new Container());


            // Here's an example of using StructureMap specific registration with a StructureMap Registry.  
            // return FubuApplication.For<MyFubuMvcPolicies>().StructureMap<MyStructureMapRegistry>();
        }
    }

    public class MyStructureMapRegistry : Registry
    {
        public MyStructureMapRegistry()
        {
            // StructureMap registration here
        }
    }

    public class MyFubuMvcPolicies : FubuRegistry
    {
        public MyFubuMvcPolicies()
        {
            // This is a DSL to change or add new conventions, policies, or application settings
        }
    }
}