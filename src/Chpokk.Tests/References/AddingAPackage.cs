using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;
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
			var targetFolder = TargetFolder;
			foreach (var directory in Directory.EnumerateDirectories(targetFolder)) {
				Console.WriteLine(directory.PathRelativeTo(targetFolder));
			}
			Directory.EnumerateDirectories(targetFolder).ShouldContain(s => s.PathRelativeTo(targetFolder).StartsWith("elmah."));
		}

		IPackage FindPackage(string packageName, IEnumerable<IPackage> source) {
			return source.First(package => package.Id == packageName);
		}

		[Test]
		public void AddsPackageContent() {
			
		}

		[Test]
		public void AddsAssemblyReference() {
			string assemblyPath = null;
			var packages = Context.Container.Get<PackageFinder>().FindPackages("elmah");
			var elmahPackage = FindPackage("elmah", packages);
			var references = new Dictionary<IPackage, IEnumerable<string>>();
			CollectPackageDependencies(references, elmahPackage, packages);
			foreach (var dependentPackage in references.Keys) {
				if (references[dependentPackage].Any()) {
					var assemblyRelativePath = references[dependentPackage].First();
					assemblyPath = Path.Combine(TargetFolder, string.Concat(dependentPackage.Id, ".", dependentPackage.Version), assemblyRelativePath);
				}
			}
			//var dependencies = elmahPackage.DependencySets.SelectMany(set => set.Dependencies);
			//foreach (var dependency in dependencies) {
			//	var dependentPackage = FindPackage(dependency.Id, packages);
			//	var assemblies = dependentPackage.AssemblyReferences;
			//	foreach (var assemblyReference in assemblies) {
			//		assemblyPath = Path.Combine(TargetFolder, string.Concat(dependentPackage.Id, ".", dependentPackage.Version), assemblyReference.Path);
			//	}
			//}
			assemblyPath.ShouldNotBe(null);
			File.Exists(assemblyPath).ShouldBe(true);
		}

		public void CollectPackageDependencies(IDictionary<IPackage, IEnumerable<string>> collection, IPackage mainPackage,
		                                                                         IEnumerable<IPackage> allPackages) {
			var assemblies = mainPackage.AssemblyReferences;
			collection.Add(mainPackage, from assembly in assemblies select assembly.Path);

			var dependencies = mainPackage.DependencySets.SelectMany(set => set.Dependencies);
			foreach (var dependency in dependencies) {
				var dependentPackage = FindPackage(dependency.Id, allPackages);
				CollectPackageDependencies(collection, dependentPackage, allPackages);
			}
		}

		public override void Act() {
			//CThruEngine.AddAspect(new DebugAspect(info => info.MethodName == "CreateAggregateRepositoryFromSources"));
			var initializer = Context.Container.Get<NuGetInitializer>();
			var command = initializer.CreateObject<InstallCommand>();
			command.OutputDirectory = TargetFolder;
			command.Source.Add(NuGetConstants.DefaultFeedUrl);
			command.Arguments.Add("elmah");
			command.ExecuteCommand();
		}

		private string TargetFolder {
			get { return Context.SolutionFolder.AppendPath("packages"); }
		}
	}
}
