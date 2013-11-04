using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class NugetPackagesEndpoint {
		private readonly PackageFinder _packageFinder;
		public NugetPackagesEndpoint(PackageFinder packageFinder) {
			_packageFinder = packageFinder;
		}

		public NugetPackagesModel DoIt(NugetPackagesInputModel model) {
			var packages = _packageFinder.FindPackages(model.Query).OrderBy(package => package.Id, StringComparer.InvariantCultureIgnoreCase);
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