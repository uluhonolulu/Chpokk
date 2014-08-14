using System.IO;
using System.Net.Mail;
using System.Web;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.CustomerDevelopment;
using ChpokkWeb.Features.CustomerDevelopment.WhosOnline;
using ChpokkWeb.Features.Editor.Intellisense.Providers;
using ChpokkWeb.Features.Editor.Menu;
using ChpokkWeb.Features.ProjectManagement;
using ChpokkWeb.Features.ProjectManagement.References.Bcl;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ChpokkWeb.Features.Remotes;
using ChpokkWeb.Features.Remotes.SaveCommit;
using ChpokkWeb.Features.Storage;
using ChpokkWeb.Features.Testing;
using Emkay.S3;
using Gallio.Runtime.ConsoleSupport;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using Microsoft.Build.Evaluation;
using NuGet;
using NuGet.Common;
using SharpSvn;
using SharpSvn.Security;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using IDependencyResolver = Microsoft.AspNet.SignalR.IDependencyResolver;

namespace ChpokkWeb.Infrastructure {
	public class ChpokkRegistry : Registry {
		public ChpokkRegistry() {
			For<Backup>().LifecycleIs(new HybridSessionLifecycle());
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
			For<KeywordProvider>().Singleton();
			For<BclAssembliesProvider>().Singleton();
			Scan(scanner =>
			{
				scanner.AssemblyContainingType<IRetrievePolicy>();
				scanner.AddAllTypesOf<IRetrievePolicy>();
				scanner.AddAllTypesOf<IEditorMenuPolicy>();
				scanner.AddAllTypesOf<ICommitter>();

				scanner.WithDefaultConventions();
			});
			//NuGet
			For<IPackageRepository>().Singleton().Use(() => PackageRepositoryFactory.Default.CreateRepository(NuGetConstants.DefaultFeedUrl));
			For<SignalRLogger>().LifecycleIs(new HybridLifecycle());
			For<IConsole>().Use(context => context.GetInstance<SignalRLogger>());
			For<IFileSystem>()
				.Use(context => new PhysicalFileSystem(Directory.GetCurrentDirectory()) {Logger = context.GetInstance<IConsole>()});
			//SignalR
			For<IDependencyResolver>().Singleton().Use<StructureMapResolver>();
			For<HttpContextBase>().Use(context => new HttpContextWrapper(context.GetInstance<HttpContext>()));

			For<IAppRootProvider>().Use<AspNetAppRootProvider>();

			//For<CredentialsCache>().LifecycleIs(new HybridSessionLifecycle());
			For<SvnClient>().Use(() =>
			{
				var client = new SvnClient();
				client.Authentication.SslServerTrustHandlers += delegate(object sender, SvnSslServerTrustEventArgs e)
				{
					e.AcceptedFailures = e.Failures;
					e.Save = true; // Save acceptance to authentication store
				};
				return client;
			});

			//tracks who's online
			For<WhosOnlineTracker>().Singleton();

			//Gallio output
			For<IRichConsole>().Use<WebConsole>();
		}
	}
}