using System;
using System.Collections.Generic;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using MbUnit.Framework;
using NuGet;
using Shouldly;
using System.Linq;
using UnitTests.Infrastructure;

namespace Chpokk.Tests.References {
	[TestFixture]
	public class ListingPackages : BaseQueryTest<SimpleConfiguredContext, IEnumerable<IPackage>> {
		[Test]
		public void SearchingForElmahReturnsElmahPackage() {
			foreach (var package in Result) {
				//Console.WriteLine(package);
			}
			Result.ShouldContain(package => package.Id == "elmah");
		}

		[Test, DependsOn("SearchingForElmahReturnsElmahPackage")]
		public void ShouldBeOnlyOnePackageForEachId() {
			Result.Count(package => package.Id == "elmah").ShouldBe(1);
		}

		public override IEnumerable<IPackage> Act() {
			var packageFinder = Context.Container.Get<PackageFinder>();
			const string searchTerm = "elmah"; //searching for "elma" returns empty list, just like with the official search
			packageFinder.FindPackages(searchTerm);
			return packageFinder.FindPackages(searchTerm);


		}

	}
}
