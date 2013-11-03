using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using NuGet;
using NuGet.Commands;
using Shouldly;

namespace Chpokk.Tests.References {
	[TestFixture]
	public class ListingPackages: BaseQueryTest<SimpleConfiguredContext, IEnumerable<IPackage>> {
		[Test]
		public void SearchingForElmahReturnsElmahPackage() {
			Result.ShouldContain(package => package.Id == "elmah");
		}

		public override IEnumerable<IPackage> Act() {
			var initializer = Context.Container.Get<NuGetInitializer>();
			var listCommand = initializer.CreateObject<ListCommand>();
			listCommand.Source.Add(NuGetConstants.DefaultFeedUrl);
			listCommand.Arguments.Add("elmah");
			//Program.Main(new string[] {""});
			return listCommand.GetPackages();
		}

	}
}
