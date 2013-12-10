using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
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
			CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("Atom") || info.MethodName.StartsWith("Resolve"), @"C:\pack.txt"));
			CThruEngine.StartListening();
			var packageFinder = Context.Container.Get<PackageFinder>();
			const string searchTerm = "elma";
			return packageFinder.FindPackages(searchTerm);


		}

	}
}
