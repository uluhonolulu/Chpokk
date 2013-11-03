using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Reflection;
using NuGet;
using NuGet.Commands;
using NuGet.Common;

namespace Chpokk.Tests.References {
	public class NuGetInitializer {
		private readonly IFileSystem _fileSystem;
		private readonly IConsole _console;
		public NuGetInitializer(IFileSystem fileSystem, IConsole console) {
			this._fileSystem = fileSystem;
			this._console = console;
		}

		public T CreateObject<T>() where T : new() {
			var target = new T();
			using (var catalog = new AggregateCatalog(new ComposablePartCatalog[]{new AssemblyCatalog(typeof(Program).Assembly)})) {
				using (var container = new CompositionContainer(catalog, new ExportProvider[0])) {
					container.ComposeExportedValue(_console);
					container.ComposeExportedValue((IPackageRepositoryFactory)new CommandLineRepositoryFactory(_console));
					container.ComposeExportedValue(_fileSystem);
					container.ComposeParts(new[]{(object) target});
				}
			}
			var command = target as Command;
			if (command != null) {
				InitializeCommand(command);
			}
			return target;
		}

		private void InitializeCommand(Command command) {
			//accessing private fields via reflection
			var settings = !string.IsNullOrEmpty(command.ConfigFile) ? Settings.LoadDefaultSettings((IFileSystem) new PhysicalFileSystem(Path.GetDirectoryName(Path.GetFullPath(command.ConfigFile))), Path.GetFileName(command.ConfigFile), command.MachineWideSettings) : Settings.LoadDefaultSettings(command.FileSystem, null, command.MachineWideSettings);
			typeof(Command).GetProperty("Settings", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(command, settings);
			var sourceProvider = typeof(Command).Assembly.GetType("NuGet.PackageSourceBuilder").GetMethod("CreateSourceProvider", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { settings });
			typeof(Command).GetProperty("SourceProvider", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(command, sourceProvider);
			typeof(Command).GetProperty("RepositoryFactory", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(command, new CommandLineRepositoryFactory(command.Console));
			//command.RepositoryFactory = new CommandLineRepositoryFactory(command.Console);
		}
	}
}