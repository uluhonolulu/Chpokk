using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;
using Microsoft.Build.Construction;
using Mono.Collections.Generic;
using NuGet;
using NuGet.Commands;
using NuGet.Common;
using Shouldly;
using Console = System.Console;

namespace Chpokk.Tests.References {
	[TestFixture]
	public class AddingAPackage : BaseCommandTest<ProjectFileContext> {
		[Test]
		public void CreatesThePackageFolder() {
			Directory.EnumerateDirectories(TargetFolder).ShouldContain(s => s.PathRelativeTo(TargetFolder).StartsWith("elmah."));
		}

		IPackage FindPackage(string packageName) {
			return Context.Container.Get<PackageInfoCache>()[packageName];
		}

		[Test]
		public void AddsPackageContent() {
			
		}

		[Test]
		public void AddsAssemblyReference() {
			var elmahPackage = FindPackage("elmah");
			var walker = Context.Container.Get<PackageDependencyWalker>();
			var references = walker.GetDependentAssemblyPaths(elmahPackage);
			var assemblyPath = TargetFolder.AppendPath(references.First());

			assemblyPath.ShouldNotBe(null);
			File.Exists(assemblyPath).ShouldBe(true);

			var project = ProjectRootElement.Open(Context.ProjectPath);
			var parser = Context.Container.Get<ProjectParser>();
			foreach (var reference in references) {
				parser.AddReference(project, TargetFolder.AppendPath(reference));
			}
			var projectReferences = project.Items.Where(element => element.ItemType == "Reference");
			projectReferences.First().Include.ShouldBe("elmah", Case.Insensitive);
		}



		public override void Act() {
			//keep packages from the search session
			var cache = Context.Container.Get<PackageInfoCache>();
			var packages = Context.Container.Get<PackageFinder>().FindPackages("elmah");
			cache.Keep(packages);

			//CThruEngine.AddAspect(new DebugAspect(info => info.MethodName == "CreateAggregateRepositoryFromSources"));
			var initializer = Context.Container.Get<NuGetInitializer>();
			var command = initializer.CreateObject<InstallCommand>();
			command.OutputDirectory = TargetFolder;
			command.Source.Add(NuGetConstants.DefaultFeedUrl);
			command.Arguments.Add("elmah");
			command.ExecuteCommand();

			//add the damn reference

		}

		private string TargetFolder {
			get { return Context.SolutionFolder.AppendPath("packages"); }
		}
	}
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
