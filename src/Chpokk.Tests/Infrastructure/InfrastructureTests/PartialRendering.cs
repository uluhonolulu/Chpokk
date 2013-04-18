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
		[Test]
		public void OutputsTheActionOutput() {
			Assert.AreEqual("hi", Result);
		}

		public override string Act() {
			var writer = Context.Container.Get<IOutputWriter>();
			var factory = Context.Container.Get<IPartialFactory>();
			var behaviorFactory = Context.Container.Get<IBehaviorFactory>();
			var arguments = Context.Container.Get<ServiceArguments>();
			//var graph = Context.Container.Get<BehaviorGraph>();
			//graph.Behaviors.Where(chain => chain.FirstCall() != null).Each(chain => Console.Write(chain.FirstCall().HandlerType));
			//var ch = graph.Behaviors.FirstOrDefault(chain => chain.FirstCall() != null && chain.FirstCall().HandlerType == typeof(SampleHandler));
			//var newChain = BehaviorChain.For<SampleHandler>(handler => handler.TellMe());
			//graph.AddChain(newChain);
			//Console.WriteLine(newChain.UniqueId);
			//var id = newChain.UniqueId;
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IOutputWriter));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IRecordedOutput));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IHttpWriter));
			var spy = new Spy<string>(info => info.TargetInstance is IHttpWriter && info.MethodName == "Write",
			                          args => (string) args.ParameterValues[0]);
			//CThruEngine.AddAspect(new DebugAspect(info => info.TargetInstance is OutputWriter && info.MethodName == "Write"));
			CThruEngine.AddAspect(spy);
			CThruEngine.StartListening();
			var actionBehavior = behaviorFactory.BuildBehavior(arguments, Context.ChainId);
			actionBehavior.InvokePartial();
			//Assert.IsNull(x);
			return spy.Results.FirstOrDefault();
			//return writer.Record(() => actionBehavior.InvokePartial()).GetText();
			return "hi";
			return ModellessPartialExtension.RenderAction<SampleHandler>("TellMe", writer, factory);
		}

		public class SampleHandler {
			public string TellMe() {
				return "hi";
			}
		}
	}

	public class NulloHttpContext: SimpleConfiguredContext {
		protected override void ConfigureFubuRegistry(ChpokkWeb.ConfigureFubuMVC registry) {
			base.ConfigureFubuRegistry(registry);
			registry.Configure(graph =>
			{
				var behaviorChain = BehaviorChain.For<PartialRendering.SampleHandler>(handler => handler.TellMe());
				ChainId = behaviorChain.UniqueId;
				behaviorChain.IsPartialOnly = true;
				graph.AddChain(behaviorChain);
			});
			registry.Services(serviceRegistry => serviceRegistry.AddService<IHttpWriter, NulloHttpWriter>());
		}

		public Guid ChainId { get; set; }
	}


}
