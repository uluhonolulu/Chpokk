using System;
using System.Collections.Generic;
using System.IO;
using FubuCore.Util;
using NuGet;
using System.Linq;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class PackageInfoCache {
		private PackageFinder _packageFinder;
		private readonly Cache<string, IPackage> _packages;
		public PackageInfoCache(PackageFinder packageFinder) {
			_packageFinder = packageFinder;
			_packages = new Cache<string, IPackage>(s => FindPackage(s));
		}

		private IPackage FindPackage(string packageId) {
			var packages = _packageFinder.FindPackages(packageId);
			if (!packages.Any(package => package.Id == packageId)) {
				var arrayOfPackages = packages.ToArray();
				throw new InvalidDataException("Can't find package " + packageId);
			}
			return packages.First(package => package.Id == packageId);
		}

		public void Keep(IEnumerable<IPackage> packages) {
			foreach (var package in packages) {
				_packages[package.Id] = package;
			}
		}



		public IPackage this[string id] { get { return _packages[id]; } }
	}
}