using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using NuGet;
using Shouldly;
using System.Linq;

namespace Chpokk.Tests.References {
	[TestFixture]
	public class ListingPackages: BaseQueryTest<SimpleConfiguredContext, IEnumerable<IPackage>> {
		[Test]
		public void SearchingForElmahReturnsElmahPackage() {
			Result.ShouldContain(package => package.Id == "elmah");

			TrackDependencies("elmah");
			TrackDependencies("elmah.corelibrary");
		}

		private void TrackDependencies(string packageName) {
			Console.WriteLine();
			Console.WriteLine(packageName);
			var tracedPackage = Result.First(package => package.Id == packageName);
			Console.WriteLine("Assembly references");
			foreach (var assemblyReference in tracedPackage.AssemblyReferences) Console.WriteLine(assemblyReference);
			Console.WriteLine("Dependency sets");
			foreach (var dependency in tracedPackage.DependencySets.SelectMany(set => set.Dependencies)) Console.WriteLine(dependency);
			Console.WriteLine("Framework assemblies");
			foreach (var frameworkAssembly in tracedPackage.FrameworkAssemblies) Console.WriteLine(frameworkAssembly);
			Console.WriteLine("Lib files");
			foreach (var libFile in tracedPackage.GetLibFiles()) Console.WriteLine(libFile);
		}

		public override IEnumerable<IPackage> Act() {
			return Context.Container.Get<PackageFinder>().FindPackages("elmah");
		}

	}
}
