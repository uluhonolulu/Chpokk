using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using CThru;
using CThru.BuiltInAspects;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ICSharpCode.PackageManagement.Cmdlets;
using MbUnit.Framework;
using Mono.Collections.Generic;
using NuGet;
using NuGet.Commands;
using NuGet.Common;
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
			var command = new NuGetInitializer(fileSystem, console).CreateObject<ListCommand>();
			command.FileSystem.ShouldBe(fileSystem);
		}
		//usik pusik
	}
}
