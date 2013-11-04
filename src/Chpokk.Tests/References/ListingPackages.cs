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
		}

		public override IEnumerable<IPackage> Act() {
			return Context.Container.Get<PackageFinder>().FindPackages("elmah");
		}

	}
}
