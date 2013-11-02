using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using CThru;
using CThru.BuiltInAspects;
using ICSharpCode.PackageManagement.Cmdlets;
using MbUnit.Framework;
using Mono.Collections.Generic;
using NuGet;
using NuGet.Commands;
using NuGet.Common;
using Shouldly;

namespace Chpokk.Tests.References {
	public class NuGetFeatures {
		private const string TARGET_FOLDER =
			@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Twitter\Chpokk-SampleSol\src";// @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\ulu\temp";
		[Test]
		public void CanGetTheListOfPackagesSearchingForElmah() {
			//PhysicalFileSystem physicalFileSystem = new PhysicalFileSystem(Directory.GetCurrentDirectory());
			//program.Manager.RegisterCommand(command);
			//var provider = new PackageSourceProvider(new Settings(new PhysicalFileSystem()))
			var command = new ListCommand()
				{
					Console = Console,
					Manager = new CommandManager()
				};
			command.Source.Add(NuGetConstants.DefaultFeedUrl);
			command.Arguments.Add("elmah");
			var packages = command.GetPackages();

			packages.Select(package => package.Id).ShouldContain("elmah");
			//Program.Main(new[]{"list", "elmah"});
		}
		[Test]
		public void ExecutingTheInstallCommandInstallsThePackage() {

			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is PackageOperation, 2));
			CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IPackageManager));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IPackageRepository, 2));

			var command = new InstallCommand()
				{
					Console = Console,
					Manager = new CommandManager(),
					OutputDirectory = TARGET_FOLDER
				};
			command.Arguments.Add("elmah");
			CThruEngine.StartListening();
			command.ExecuteCommand();

			Directory.EnumerateDirectories(TARGET_FOLDER).ShouldContain(s => s.Contains("elmah"));
		}

		[TearDown]
		public void Cleanup() {
			if (Directory.Exists(TARGET_FOLDER)) {
				//Directory.Delete(TARGET_FOLDER, true);
			}
		}

		private static NuGet.Common.Console Console {
			get { return new NuGet.Common.Console(); }
		}

		[Test]
		public void RunningSharpDevelopVersion() {
			var installPackageCmdlet = new InstallPackageCmdlet() { Id = "nunit", Solution = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Twitter\Chpokk-SampleSol\src\ChpokkSampleSolution.sln" };
			var results = installPackageCmdlet.Invoke();
			foreach (var result in results) {
				System.Console.WriteLine(result);
			}
		}

		[Test]
		public void InitializingTheCommands() {
			var console = new NuGet.Common.Console(){Verbosity = Verbosity.Normal};
			var fileSystem = new PhysicalFileSystem(Directory.GetCurrentDirectory()){Logger = console};
			var program = new Program();
			Initialize(fileSystem, console, program);
			program.Commands.ShouldContain(command => command is ListCommand);
		}

		private static void Initialize(IFileSystem fileSystem, IConsole console, object target) {
			using (var catalog = new AggregateCatalog(new ComposablePartCatalog[]
			  {
				new AssemblyCatalog(typeof(Program).Assembly)
			  })) {

				using (var container = new CompositionContainer(catalog, new ExportProvider[0])) {
					AttributedModelServices.ComposeExportedValue<IConsole>(container, console);
					AttributedModelServices.ComposeExportedValue<IPackageRepositoryFactory>(container, (IPackageRepositoryFactory)new CommandLineRepositoryFactory(console));
					AttributedModelServices.ComposeExportedValue<IFileSystem>(container, fileSystem);
					//AttributedModelServices.ComposeExportedValue<ICommandManager>(container, new CommandManager());
					//AttributedModelServices.ComposeExportedValue<HelpCommand>(container, new HelpCommand(new CommandManager()));
					AttributedModelServices.ComposeParts(container, new object[1]
          {
            target
          });
				}
			}

			
		}
	}
}
