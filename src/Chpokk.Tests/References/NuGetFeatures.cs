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
	public class NuGetFeatures {
		[Test]
		public void LetsSee() {
			var console = new NuGet.Common.Console();
			//PhysicalFileSystem physicalFileSystem = new PhysicalFileSystem(Directory.GetCurrentDirectory());
			//program.Manager.RegisterCommand(command);
			var settings = Settings.LoadDefaultSettings();
			var sourceProvider = new PackageSourceProvider(settings, new[]{new PackageSource(NuGetConstants.DefaultFeedUrl)});
			//var provider = new PackageSourceProvider(new Settings(new PhysicalFileSystem()))
			var command = new ListCommand(new CommandLineRepositoryFactory(), sourceProvider)
				{
					Console = console,
					Manager = new CommandManager()
				};
			command.Arguments.Add("elmah");
			var packages = command.GetPackages();

			foreach (var package in packages) {
				Console.WriteLine(package);
			}
			//Program.Main(new[]{"list", "elmah"});
		}
	}
}
