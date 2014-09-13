﻿using System;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.CustomerDevelopment.Pricelist;
using FubuMVC.Core.Registration;
using MbUnit.Framework;
using Shouldly;
using System.Linq;

namespace UnitTests.Infrastructure {
	[TestFixture, Ignore("Until AppHarbox can handle this")]//
	public class RouteList: BaseCommandTest<SimpleConfiguredContext> {
		[Test]
		public void SeeTheConsoleOutput() {
			//
			// 
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
