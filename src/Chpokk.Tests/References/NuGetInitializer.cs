using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using NuGet;
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
			return target;
		}
	}
}