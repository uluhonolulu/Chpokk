using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CThru;
using CThru.BuiltInAspects;
using MbUnit.Framework;
using NuGet;
using NuGet.Commands;
using NuGet.Common;
using Console = System.Console;

namespace Chpokk.Tests.Spikes {
	public class GitSpike {
		[Test]
		public void LetsSee() {
			//NuGet.
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is ListCommand));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is CommandLineRepositoryFactory));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is ISettings));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is PackageSourceProvider, 4).DisplayFor<PackageSource[]>(sources =>
			//{

			//	var builder = new StringBuilder();
			//	foreach(var packageSource in sources) {
			//		builder.AppendLine(packageSource.ToString());
			//	}
			//	return builder.ToString();
			//}));
			//CThruEngine.StartListening();
			NuGet.Common.Console console = new NuGet.Common.Console();
			//PhysicalFileSystem physicalFileSystem = new PhysicalFileSystem(Directory.GetCurrentDirectory());
			//program.Manager.RegisterCommand(command);
			var settings = Settings.LoadDefaultSettings();
			var sourceProvider = new PackageSourceProvider(settings, new[]{new PackageSource(NuGetConstants.DefaultFeedUrl)});
			//var provider = new PackageSourceProvider(new Settings(new PhysicalFileSystem()))
			var command = new ListCommand(new CommandLineRepositoryFactory(), sourceProvider);
			command.Console = console;
			command.Manager = new CommandManager();
			command.Arguments.Add("elmah");
			var packages = command.GetPackages();

			foreach (var package in packages) {
				Console.WriteLine(package);
			}
			//Program.Main(new[]{"list", "elmah"});
		}
	}
}
