using System;
using System.Collections.Generic;
using NuGet;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class PackageInfoCache {
		private readonly IDictionary<string, IPackage> _packages = new Dictionary<string, IPackage>();
		public void Keep(IEnumerable<IPackage> packages) {
			foreach (var package in packages) {
				_packages[package.Id] = package;
				Console.WriteLine("Adding " + package.Id);
			}
		}

		public IPackage this[string id] { get { return _packages[id]; } }
	}
}