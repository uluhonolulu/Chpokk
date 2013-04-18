using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using ChpokkWeb.Infrastructure;
using FubuCore.Binding;
using FubuMVC.Core.Caching;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;

namespace Chpokk.Tests.Infrastructure.InfrastructureTests {
	[TestFixture]
	public class PartialRendering : BaseQueryTest<NulloHttpContext, string> {
		public static readonly string _expectedValue = "hi";

		[Test]
		public void OutputsTheActionOutput() {
			Assert.AreEqual(Result, _expectedValue);
		}

		public override string Act() {
			var spy = new Spy<string>(info => info.TargetInstance is IHttpWriter && info.MethodName == "Write",
			                          args => (string) args.ParameterValues[0]);
			CThruEngine.AddAspect(spy);
			CThruEngine.StartListening();
			var behaviorFactory = Context.Container.Get<IBehaviorFactory>();
			var arguments = Context.Container.Get<ServiceArguments>();
			var graph = Context.Container.Get<BehaviorGraph>();
			var writer = Context.Container.Get<IOutputWriter>();
			var methodName = "TellMe";
			var thisChain =
				graph.Behaviors.FirstOrDefault(
					chain =>
					{
						var actionCall = chain.FirstCall();
						return actionCall != null && actionCall.HandlerType == typeof (SampleHandler) && actionCall.Method.Name == methodName;
					});
			var actionBehavior = behaviorFactory.BuildBehavior(arguments, thisChain.UniqueId);
			;
			//return spy.Results.FirstOrDefault();
			return writer.Record(() => actionBehavior.InvokePartial()).GetText();
		}

		public class SampleHandler {
			public string TellMe() {
				return _expectedValue;
			}
		}
	}

	public class NulloHttpContext: SimpleConfiguredContext {
		protected override void ConfigureFubuRegistry(ChpokkWeb.ConfigureFubuMVC registry) {
			base.ConfigureFubuRegistry(registry);
			registry.Configure(graph =>
			{
				var behaviorChain = BehaviorChain.For<PartialRendering.SampleHandler>(handler => handler.TellMe());
				//ChainId = behaviorChain.UniqueId;
				behaviorChain.IsPartialOnly = true;
				graph.AddChain(behaviorChain);
			});
			registry.Services(serviceRegistry => serviceRegistry.AddService<IHttpWriter, NulloHttpWriter>());
		}

	}


}
