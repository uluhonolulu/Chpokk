using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using FubuCore;
using Microsoft.Build.Construction;
using NuGet;
using NuGet.Commands;

namespace Chpokk.Tests.References {
	public class PackageInstaller {
		private readonly PackageDependencyWalker _walker;
		private readonly PackageInfoCache _cache;
		private readonly NuGetInitializer _initializer;
		private readonly ProjectParser _parser;
		public PackageInstaller(PackageDependencyWalker walker, PackageInfoCache cache, NuGetInitializer initializer, ProjectParser parser) {
			this._walker = walker;
			this._cache = cache;
			this._initializer = initializer;
			this._parser = parser;
		}

		public void InstallPackage(string id, string targetFolder, string projectPath) {
			var command = _initializer.CreateObject<InstallCommand>();
			command.OutputDirectory = targetFolder;
			command.Source.Add(NuGetConstants.DefaultFeedUrl);
			command.Arguments.Add(id);
			command.ExecuteCommand();
			
			var project = ProjectRootElement.Open(projectPath);

			//add the damn references
			var elmahPackage = _cache[id];
			var references = _walker.GetDependentAssemblyPaths(elmahPackage);
			foreach (var reference in references) {
				_parser.AddReference(project, targetFolder.AppendPath(reference));
			}
		}
	}
}