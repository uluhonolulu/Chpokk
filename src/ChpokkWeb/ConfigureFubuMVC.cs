using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ChpokkWeb.App_Start;
using ChpokkWeb.Repa;
using ChpokkWeb.Shared;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.View;
using FubuMVC.Spark;
using FubuMVC.Spark.SparkModel;
using StructureMap;
using dotless.Core;
using dotless.Core.Importers;
using dotless.Core.Input;
using dotless.Core.Parser;
using dotless.Core.Stylizers;

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

			//Services(registry => registry.ReplaceService<ILessEngine>(new LessEngine(new Parser(new PlainStylizer(), new Importer(new FileReader(new AssetPathResolver()))){})));

			Views
				.TryToAttachWithDefaultConventions()
				.RegisterActionLessViews(
				token => typeof(IDontNeedActionsModel).IsAssignableFrom(token.ViewModelType) || token.ViewModelType.Name.Contains("InputModel"), (chain, token) =>
				        {
				            var url = (token.Name == "DemoView") ? "" : token.Name;
							if (token.ViewModelType == typeof(RepositoryInputModel))
				        	{
				        		url += "/{Name}";
				        	}
				            chain.Route = new RouteDefinition(url);
				        })
				;
		}

		internal class AssetPathResolver : IPathResolver {
			public string GetFullPath(string path) {
				return HttpContext.Current.Server.MapPath(path).Replace(@"\_content\", @"\Content\");
			}
		}
	}


}