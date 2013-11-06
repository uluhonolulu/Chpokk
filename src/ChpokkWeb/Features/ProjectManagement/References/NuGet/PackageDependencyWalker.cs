using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class PackageDependencyWalker {
		private readonly PackageInfoCache _cache;
		public PackageDependencyWalker(PackageInfoCache cache) {
			_cache = cache;
		}

		public IEnumerable<string> GetDependentAssemblyPaths(IPackage package) {
			var references = this.CollectPackageDependencies(package);
			return this.GetDependentAssemblyPaths(references);
		}

		public IDictionary<IPackage, IEnumerable<string>> CollectPackageDependencies(IPackage mainPackage) {
			var references = new Dictionary<IPackage, IEnumerable<string>>();
			CollectPackageDependencies(references, mainPackage);
			return references;
		}

		private void CollectPackageDependencies(IDictionary<IPackage, IEnumerable<string>> collection, IPackage mainPackage) {
			var assemblies = mainPackage.AssemblyReferences;
			collection.Add(mainPackage, from assembly in assemblies select assembly.Path);

			var dependencies = mainPackage.DependencySets.SelectMany(set => set.Dependencies);
			foreach (var dependency in dependencies) {
				var dependentPackage = _cache[dependency.Id];
				CollectPackageDependencies(collection, dependentPackage);
			}
		}

		public IEnumerable<string> GetDependentAssemblyPaths(IDictionary<IPackage, IEnumerable<string>> dependencies) {
			foreach (var package in dependencies.Keys) {
				foreach (var assemblyRelativePath in dependencies[package])
					yield return GetPackageAssemblyPath(package, assemblyRelativePath);
			}
		}

		public string GetPackageAssemblyPath(IPackage package, string assemblyRelativePath) {
			return Path.Combine(string.Concat(package.Id, ".", package.Version), assemblyRelativePath);
				
		}

	}
}