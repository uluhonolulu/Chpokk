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
			const string searchTerm = "elma";
			return packageFinder.FindPackages(searchTerm);


		}

	}
}
