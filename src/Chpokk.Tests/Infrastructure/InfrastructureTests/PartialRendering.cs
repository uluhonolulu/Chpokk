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
			Assert.AreEqual(_expectedValue, Result);
		}

		public override string Act() {
			var spy = new Spy<string>(info => info.TargetInstance is IOutputWriter && info.MethodName == "Write",
									  args =>
									  {
										  var index = (args.ParameterValues.Length == 1) ? 0 : 1; 
										  return (string)args.ParameterValues[index]; 
									  });
			CThruEngine.AddAspect(spy);
			CThruEngine.StartListening();
			var partialFactory = Context.Container.Get<IPartialFactory>();
			var arguments = Context.Container.Get<ServiceArguments>();
			var graph = Context.Container.Get<BehaviorGraph>();
			var writer = Context.Container.Get<IOutputWriter>();
			var methodName = "TellMe";
			var result = ModellessPartialExtension.Partial<SampleHandler>(methodName, partialFactory, graph, arguments, writer);
			return spy.Results.FirstOrDefault();
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
