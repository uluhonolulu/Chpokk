using System;
using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.Demo;
using ChpokkWeb.Features.Editor;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.MainScreen;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Assets.Content;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Routes;
using Spark;

namespace ChpokkWeb {
	public class ConfigureFubuMVC : FubuRegistry {
		public ConfigureFubuMVC() {


			// All public methods from concrete classes ending in "Controller"
			// in this assembly are assumed to be action methods
			Actions
				.IncludeClassesSuffixedWithController()
				.IncludeClassesSuffixedWithEndpoint();

			// Policies
			Routes
				.IgnoreControllerNamesEntirely()
				.RootAtAssemblyNamespace()
				.IgnoreNamespaceText("Features")
				.IgnoreNamespaceText("Infrastructure")
				.IgnoreMethodsNamed("DoIt")
				.HomeIs<DemoModel>()
				;



			Routes.HomeIs<DemoModel>();

			Services(registry =>
			         {
			         	registry.FillType<IModelUrlResolver, ModelUrlResolutionCache>();
			         	registry.AddService<ITransformerPolicy>(
			         		new JavascriptTransformerPolicy<UrlTransformer>(ActionType.Transformation, ".js"));
			         });

			Policies.Add<DownloadDataConvention>();
			//Configure(graph =>
			//{
			//	Console.WriteLine("==================================");
			//	Console.WriteLine();
			//	graph.Actions().Each(call => call.WrapWith<SignoutJohnDoeBehavior>());
			//}); // if the user is authenticated, but not in the database, force it to log out so that it signs in via Janrain
			//ApplyConvention<AjaxExceptionWrappingConvention>();
			//Policies.Add<SignoutJohnDoeConfiguration>();
			var policy = new Policy();
			policy.Where.InputTypeIs<MainDummyModel>();
			policy.Wrap.WithBehavior<SignoutJohnDoeBehavior>();
			Policies.Add(policy, "InjectNodes");
		}

		//internal class AssetPathResolver : IPathResolver {
		//    public string GetFullPath(string path) {
		//        return HttpContext.Current.Server.MapPath(path).Replace(@"\_content\", @"\Content\");
		//    } 
		//}

		//public class SparkSettingsActivator : IActivator {
		//    private readonly ISparkViewEngine _engine;
		//    public SparkSettingsActivator(ISparkViewEngine engine) {
		//        _engine = engine;
		//    }

		//    public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log) {
		//        var settings = _engine.Settings.As<SparkSettings>();
		//        settings.Debug = true;
		//    }
		//}
	}


}