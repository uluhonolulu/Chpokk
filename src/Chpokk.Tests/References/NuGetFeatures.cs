using System.IO;
using CThru;
using CThru.BuiltInAspects;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using MbUnit.Framework;
using NuGet;
using NuGet.Commands;
using Shouldly;

namespace Chpokk.Tests.References {
	public class NuGetFeatures {
		private const string TARGET_FOLDER =  @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\ulu\temp";

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
				Directory.Delete(TARGET_FOLDER, true);
			}
		}

		private static NuGet.Common.Console Console {
			get { return new NuGet.Common.Console(); }
		}



		[Test]
		public void InitializingTheCommands() {
			var console = new NuGet.Common.Console(){Verbosity = Verbosity.Normal};
			var fileSystem = new PhysicalFileSystem(Directory.GetCurrentDirectory()){Logger = console};
			var command = new NuGetInitializer(fileSystem, console).CreateObject<ListCommand>();
			command.FileSystem.ShouldBe(fileSystem);
		}
		//usik pusik
	}
}
