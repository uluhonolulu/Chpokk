using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Reflection;
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
			Console.WriteLine(DateTime.Now);
			Result.Count(package => package.Id == "elmah").ShouldBe(1);
			Console.WriteLine(DateTime.Now);
		}

		public override IEnumerable<IPackage> Act() {
			Console.WriteLine(DateTime.Now);
			var packageFinder = Context.Container.Get<PackageFinder>();
			const string searchTerm = "elma";
			packageFinder.FindPackages(searchTerm);
			Console.WriteLine(DateTime.Now);
			CThruEngine.AddAspect(new TimingTraceAspect(info => info.TargetInstance is IPackage || info.TypeName.EndsWith("ClientType"), @"C:\nugget.txt"));
			CThruEngine.StartListening();
			return packageFinder.FindPackages(searchTerm);


		}

	}
}
