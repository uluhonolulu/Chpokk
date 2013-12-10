using System.Collections.Generic;
using NuGet;
using NuGet.Commands;
using System.Linq;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class PackageFinder {
		private readonly IPackageRepository _packageRepository;
		public PackageFinder(IPackageRepository packageRepository) {
			_packageRepository = packageRepository;
		}

		public IEnumerable<IPackage> FindPackages(string searchTerm) {
			return _packageRepository.Search(searchTerm, false).Where(package => package.IsLatestVersion).ToArray();
		}
	}
}