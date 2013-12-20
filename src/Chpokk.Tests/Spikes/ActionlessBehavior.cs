using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb;
using ChpokkWeb.Features.MainScreen;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class ActionlessBehavior: BaseCommandTest<SimpleConfiguredContext> {
		public ActionlessBehavior() {
			CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is BehaviorGraph && info.MethodName == "AddChain", 10));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("BehaviorChain") && info.MethodName == ".ctor", 3));
			CThruEngine.StartListening();
			
		}

		[Test]
		public void Test() {
			//
			// TODO: Add test logic here
			//
		}

		public override void Act() {
			var graph = Context.Container.Get<BehaviorGraph>();
			Console.WriteLine(graph.Behaviors.Count());
			//var behavior = graph.BehaviorFor(typeof (MainDummyModel));
			//graph = BehaviorGraph.BuildFrom<ConfigureFubuMVC>();
			//Console.WriteLine(graph.Behaviors.Count());
		}

		public void QuickTest() {
			CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is BehaviorGraph && info.MethodName == "AddChain", 10));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("BehaviorChain") && info.MethodName == ".ctor", 3));
			CThruEngine.StartListening();
			var graph = BehaviorGraph.BuildFrom<ConfigureFubuMVC>();
			//var behavior = graph.BehaviorFor(typeof (MainDummyModel));
			Console.WriteLine(graph.Behaviors.Count());
		}
	}

	
}
