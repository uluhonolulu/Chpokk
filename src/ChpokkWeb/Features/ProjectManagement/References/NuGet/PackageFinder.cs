using System.Collections.Generic;
using NuGet;
using NuGet.Commands;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class PackageFinder {
		private readonly NuGetInitializer _initializer;
		public PackageFinder(NuGetInitializer initializer) {
			_initializer = initializer;
		}

		public IEnumerable<IPackage> FindPackages(string searchTerm) {
			var listCommand = _initializer.CreateObject<ListCommand>();
			listCommand.Source.Add(NuGetConstants.DefaultFeedUrl);
			listCommand.Arguments.Add(searchTerm);
			return listCommand.GetPackages();
		}
	}
}