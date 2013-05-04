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
			var factory = page.Get<IServiceFactory>();
			var writer = page.Get<IOutputWriter>();
			var graph = page.Get<BehaviorGraph>();
			var serviceArguments = page.Get<ServiceArguments>();
			return Partial<THandler>(methodName, factory, graph, serviceArguments, writer);
		}

		public static string Partial<THandler>(string methodName, IServiceFactory serviceFactory, BehaviorGraph graph, ServiceArguments serviceArguments, IOutputWriter writer) {
			var thisChain =
				graph.Behaviors.FirstOrDefault(
					chain => {
						var actionCall = chain.FirstCall();
						return actionCall != null && actionCall.HandlerType == typeof(THandler) && actionCall.Method.Name == methodName;
					});
			var actionBehavior = serviceFactory.BuildBehavior(serviceArguments, thisChain.UniqueId);
			return writer.Record(() => actionBehavior.InvokePartial()).GetText();
		}
	}
}