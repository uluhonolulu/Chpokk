using System.IO;
using System.Net.Mail;
using System.Web;
using Amazon;
using Amazon.S3;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.CustomerDevelopment;
using ChpokkWeb.Features.CustomerDevelopment.TrialSignup;
using ChpokkWeb.Features.CustomerDevelopment.WhosOnline;
using ChpokkWeb.Features.Editor.Intellisense.Providers;
using ChpokkWeb.Features.Editor.Menu;
using ChpokkWeb.Features.ProjectManagement;
using ChpokkWeb.Features.ProjectManagement.ProjectTemplates;
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
			//Amazon
			For<AmazonS3Client>().Singleton()
								 .Use(new AmazonS3Client("AKIAIHOC7V5PPD4KIZBQ", "UJlRXeixN8/cQ5XuZK9USGUMzhnxsGs7YYiZpozM", RegionEndpoint.USEast1));
			For<IS3Client>().Singleton()
				.Use(context => new S3Client(context.GetInstance<AmazonS3Client>(), true));
			For<RestoreSynchronizer>().LifecycleIs(new HybridSessionLifecycle());
			For<ActivityTracker>().LifecycleIs(new HybridSessionLifecycle());
			For<UserData>().LifecycleIs(new HybridSessionLifecycle());
			For<ProjectCollection>().Singleton().Use(() => ProjectCollection.GlobalProjectCollection);
			For<KeywordProvider>().Singleton();
			For<BclAssembliesProvider>().Singleton();
			//project template cache
			For<TemplateListCache>().Singleton();

			Scan(scanner =>
			{
				scanner.AssemblyContainingType<IRetrievePolicy>();
				scanner.AddAllTypesOf<IRetrievePolicy>();
				scanner.AddAllTypesOf<IEditorMenuPolicy>();
				scanner.AddAllTypesOf<ICommitter>();

				scanner.WithDefaultConventions();
			});
			//NuGet
			For<IMachineWideSettings>().Singleton().Use<CommandLineMachineWideSettings>();
			For<ISettings>()
				.Singleton()
				.Use(context => Settings.LoadDefaultSettings(null, null, context.GetInstance<IMachineWideSettings>()));
			For<PackageSource>().Singleton().Use(context => new PackageSource(NuGetConstants.DefaultFeedUrl));
			For<IPackageSourceProvider>().Singleton().Use(context => new PackageSourceProvider(context.GetInstance<ISettings>(), new[] { context.GetInstance<PackageSource>() }));
			For<IPackageRepository>().Singleton().Use(context => context.GetInstance<IPackageSourceProvider>().CreateAggregateRepository(PackageRepositoryFactory.Default, true));



			//SignalR
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
			//timing before inviting to register
			For<ExperienceTracker>().LifecycleIs(new HybridSessionLifecycle());

			//Gallio output
			For<IRichConsole>().Use<WebGallioConsole>();
		}
	}
}