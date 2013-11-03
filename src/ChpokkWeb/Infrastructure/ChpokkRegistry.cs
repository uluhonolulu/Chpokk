using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.Compilation;
using ChpokkWeb.Features.CustomerDevelopment;
using ChpokkWeb.Features.Editor.Intellisense;
using ChpokkWeb.Features.Editor.Menu;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement;
using ChpokkWeb.Features.ProjectManagement.References;
using ChpokkWeb.Features.Remotes;
using ChpokkWeb.Features.RepositoryManagement;
using Emkay.S3;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using Microsoft.Build.Evaluation;
using NuGet;
using NuGet.Common;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using ILogger = Microsoft.Build.Framework.ILogger;

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

				scanner.WithDefaultConventions();
			});
			//NuGet
			For<IConsole>().Use(new NuGet.Common.Console());
			For<IFileSystem>()
				.Use(context => new PhysicalFileSystem(Directory.GetCurrentDirectory()) {Logger = context.GetInstance<IConsole>()});
		}
	}
}