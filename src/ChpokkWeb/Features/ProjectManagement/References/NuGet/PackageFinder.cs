using System.Collections.Generic;
using NuGet;
using System.Linq;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class PackageFinder {
		//static PackageFinder() {
		//	var dataServiceContext = new DataServiceContext(new Uri(NuGetConstants.DefaultFeedUrl));
		//	var methodInfo = typeof(DataServiceContext).GetMethod("ResolveTypeFromName", BindingFlags.Instance | BindingFlags.NonPublic);
		//	methodInfo.Invoke(dataServiceContext, new object[] { "NuGetGallery.V2FeedPackage", typeof(V2FeedPackage), false });
			
		//}
		private readonly IPackageRepository _packageRepository;
		public PackageFinder(IPackageRepository packageRepository) {
			_packageRepository = packageRepository;
		}

		public IEnumerable<IPackage> FindPackages(string searchTerm) {
			var settings = Settings.LoadDefaultSettings(null, null, new CommandLineMachineWideSettings());
			var defaultPackageSource = new PackageSource(NuGetConstants.DefaultFeedUrl);
			var packageSourceProvider = new PackageSourceProvider(
				settings
			);
			var packageRepository = packageSourceProvider.CreateAggregateRepository(PackageRepositoryFactory.Default, true);
			return packageRepository.Search(searchTerm, false).Where(package => package.IsLatestVersion).ToArray();
		}
	}
}

//fake class

//namespace NuGetGallery {
//	public class V2FeedPackage : DataServicePackage {
		
//	}

//}