using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement;
using ChpokkWeb.Features.Remotes;
using ChpokkWeb.Features.RepositoryManagement;
using Emkay.S3;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace ChpokkWeb.Infrastructure {
	public class ChpokkRegistry : Registry {
		public ChpokkRegistry() {
			For<RepositoryCache>().LifecycleIs(new HybridSessionLifecycle());
			For<ProjectContentRegistry>().Singleton();
			For<ProjectCache>().LifecycleIs(new HybridSessionLifecycle());
			For<HttpContext>().Use(() => HttpContext.Current);
			For<NRefactoryResolver>().Use(() => new NRefactoryResolver(LanguageProperties.CSharp));
			//For<ProjectFactory>().Singleton();
			For<SmtpClient>().Use(() => new SmtpClient()); //expr.SelectConstructor(() => new SmtpClient());
			For<IS3Client>().Singleton()
				.Use(new S3Client("AKIAIHOC7V5PPD4KIZBQ", "UJlRXeixN8/cQ5XuZK9USGUMzhnxsGs7YYiZpozM"));
			Scan(scanner =>
			{
				scanner.AssemblyContainingType<IRetrievePolicy>();
				scanner.AddAllTypesOf<IRetrievePolicy>();
			});
		}
	}
}