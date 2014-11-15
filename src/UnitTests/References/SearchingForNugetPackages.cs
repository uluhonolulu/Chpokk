using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Arractas;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using MbUnit.Framework;
using NuGet;
using System.Linq;
using Shouldly;
using UnitTests.Infrastructure;

namespace UnitTests.References {
	[TestFixture]
	public class SearchingForNugetPackages: BaseQueryTest<SimpleConfiguredContext, IEnumerable<IPackage> > {
		[Test]
		public void ShouldReturnSomeResults() {
			Result.Any().ShouldBe(true);
			Console.WriteLine(Result.Count());
			//foreach (var package in Result) {
			//	Console.WriteLine(package);
			//}
		}

		public override IEnumerable<IPackage> Act() {
			var packageFinder = Context.Container.Get<PackageFinder>();
			return packageFinder.FindPackages("mvc");
		}
	}


}
