using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using NuGet;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.References {
	[TestFixture]
	public class ListingInstalledPackages : BaseQueryTest<ProjectWithPackageContext, IEnumerable<IPackage>> {
		[Test]
		public void ShouldSeeOnePackage() {
			Result.Count().ShouldBe(1);
		}

		public override IEnumerable<IPackage> Act() {
			return Context.Container.Get<PackageInstaller>().GetAllPackages(Context.SolutionFolder);
		}
	}
}
