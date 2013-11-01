using System.IO;
using System.Linq;
using CThru;
using CThru.BuiltInAspects;
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

		private static PackageSourceProvider PackageSourceProvider {
			get {
				var settings = Settings.LoadDefaultSettings();
				var sourceProvider = new PackageSourceProvider(settings, new[] {new PackageSource(NuGetConstants.DefaultFeedUrl)});
				return sourceProvider;
			}
		}

		private static NuGet.Common.Console Console {
			get { return new NuGet.Common.Console(); }
		}

	}
}
