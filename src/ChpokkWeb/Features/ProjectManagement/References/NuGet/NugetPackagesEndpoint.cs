using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class NugetPackagesEndpoint {
		private readonly PackageFinder _packageFinder;
		private readonly PackageInfoCache _cache;
		public NugetPackagesEndpoint(PackageFinder packageFinder, PackageInfoCache cache) {
			_packageFinder = packageFinder;
			_cache = cache;
		}

		public NugetPackagesModel DoIt(NugetPackagesInputModel model) {
			var packages = _packageFinder.FindPackages(model.Query).OrderBy(package => package.Id, StringComparer.InvariantCultureIgnoreCase);
			_cache.Keep(packages);
			return new NugetPackagesModel{Packages = from package in packages select new NugetPackageModel(){Id = package.Id, Version = package.Version.ToString(), Description = package.Description}};
		}
	}

	public class NugetPackagesInputModel {
		public string Query { get; set; }
	}

	public class NugetPackagesModel {
		public IEnumerable<NugetPackageModel> Packages { get; set; }
	}

	public class NugetPackageModel {
		public string Id { get; set; }
		public string Version { get; set; }
		public string Description { get; set; }
	}
}