using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Chpokk.Tests.References;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.Compilation;
using ChpokkWeb.Features.CustomerDevelopment;
using ChpokkWeb.Features.Editor.Intellisense;
using ChpokkWeb.Features.Editor.Menu;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement;
using ChpokkWeb.Features.Remotes;
using ChpokkWeb.Features.RepositoryManagement;
using Emkay.S3;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
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
			For<SmtpClient>().Singleton().Use(() => new SmtpClient()); //expr.SelectConstructor(() => new SmtpClient());
			For<IS3Client>().Singleton()
				.Use(new S3Client("AKIAIHOC7V5PPD4KIZBQ", "UJlRXeixN8/cQ5XuZK9USGUMzhnxsGs7YYiZpozM"));
			For<ActivityTracker>().LifecycleIs(new HybridSessionLifecycle());
			For<UserData>().LifecycleIs(new HybridSessionLifecycle());
			For<ProjectLoader>().LifecycleIs(new HybridSessionLifecycle());
			For<ProjectCollection>().Singleton().Use(() => ProjectCollection.GlobalProjectCollection);
			For<ILogger>().Use<ChpokkLogger>();
			For<KeywordProvider>().Singleton();
			For<BclAssembliesProvider>().Singleton();
			Scan(scanner =>
			{
				scanner.AssemblyContainingType<IRetrievePolicy>();
				scanner.AddAllTypesOf<IRetrievePolicy>();
				scanner.AddAllTypesOf<IEditorMenuPolicy>();
			});
		}
	}
}