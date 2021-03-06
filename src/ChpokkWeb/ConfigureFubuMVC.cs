using System;
using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.CustomerDevelopment.TimeToPay;
using ChpokkWeb.Features.Demo;
using ChpokkWeb.Features.Editor;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.MainScreen;
using ChpokkWeb.Features.Storage;
using ChpokkWeb.Infrastructure;
using ChpokkWeb.Infrastructure.FileSystem;
using ChpokkWeb.Infrastructure.Logging;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Assets.Content;
using FubuMVC.Core.Assets.Files;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Spark;
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
						 registry.ReplaceService<IFileSystem, LocalAndRemoteFileSystem>();
			         });

			Policies.Add<DownloadDataConvention>();

			//timing for the Main screen
			//Policies.WrapWith<TimingBehavior>(); (that wraps all nodes, which is not what we need)
			var policy = new Policy();
			policy.Where.ChainMatches(chain => chain.Route != null && !(chain.Route.Pattern.StartsWith("_content")));
			policy.Wrap.WithBehavior<TimingBehavior>();
			Policies.Add(policy, "InjectNodes");

			//redirect to the new domain
			Policies.WrapWith<DomainRedirectionBehavior>();

			// if the user is authenticated, but not in the database, force it to log out so that it signs in via Janrain
			//ApplyConvention<AjaxExceptionWrappingConvention>();
			//Policies.Add<SignoutJohnDoeConfiguration>();

			this.AlterSettings <SparkEngineSettings>(settings => {
				                                                     settings.PrecompileViews = false;
			});

			// if the user is past the trial period, redirect her to the payment page
			//Policies.WrapWith<EndOfTrialTimeToPayBehavior>();
			//Policies.EnrichCallsWith<DemoBehaviorForSelectActions>(call => call.Method.Name == "Home" ); 
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