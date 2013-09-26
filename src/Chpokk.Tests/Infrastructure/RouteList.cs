using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Routing;
using Arractas;
using ChpokkWeb.Features.CustomerDevelopment.Pricelist;
using FubuMVC.Core.Registration;
using FubuMVC.Diagnostics.Routes;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;
using System.Linq;

namespace Chpokk.Tests.Infrastructure {
	[TestFixture]
	public class RouteList: BaseCommandTest<SimpleConfiguredContext> {
		[Test]
		public void SeeTheConsoleOutput() {
			//
			// TODO: Add test logic here
			//
		}

		public override void Act() {
			Context.ShouldNotBe(null);
			Context.Container.ShouldNotBe(null);
			var graph = Context.Container.Get<BehaviorGraph>();
			foreach (var behavior in graph.Behaviors.Where(chain => chain.Route != null).OrderBy(chain => chain.Route.Pattern)) {
				Console.WriteLine(behavior.Route);
			}
			Console.WriteLine(graph.Behaviors.Any(chain => chain.InputType() == typeof(PriceListInputModel)));
		}
	}
}
