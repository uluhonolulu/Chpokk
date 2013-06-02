using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement;
using Emkay.S3;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace ChpokkWeb.Infrastructure {
	public class ChpokkRegistry : Registry {
		public ChpokkRegistry() {
			For<RepositoryManager>().LifecycleIs(new HybridSessionLifecycle());
			For<HttpContext>().Use(() => HttpContext.Current);
			For<NRefactoryResolver>().Use(() => new NRefactoryResolver(LanguageProperties.CSharp));
			For<ProjectFactory>().Singleton();
			For<IS3Client>()
				.Use(
					context =>
					context.GetInstance<S3ClientFactory>().Create("AKIAIHOC7V5PPD4KIZBQ", "UJlRXeixN8/cQ5XuZK9USGUMzhnxsGs7YYiZpozM"));
		}
	}
}