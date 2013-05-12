using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore.Binding;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.View;

namespace ChpokkWeb.Infrastructure {
	public static class ModellessPartialExtension {
		public static string Partial<THandler>(this IFubuPage page, string methodName) {
			var factory = page.Get<IPartialFactory>();
			var writer = page.Get<IOutputWriter>();
			var graph = page.Get<BehaviorGraph>();
			var serviceArguments = page.Get<ServiceArguments>();
			return Partial<THandler>(methodName, factory, graph, serviceArguments, writer);
		}

		public static string Partial<THandler>(string methodName, IPartialFactory partialFactory, BehaviorGraph graph, ServiceArguments serviceArguments, IOutputWriter writer) {
			var thisChain =
				graph.Behaviors.FirstOrDefault(
					chain => {
						var actionCall = chain.FirstCall();
						return actionCall != null && actionCall.HandlerType == typeof(THandler) && actionCall.Method.Name == methodName;
					});
			var actionBehavior = partialFactory.BuildBehavior(thisChain);
			return writer.Record(() => actionBehavior.InvokePartial()).GetText();
		}
	}
}