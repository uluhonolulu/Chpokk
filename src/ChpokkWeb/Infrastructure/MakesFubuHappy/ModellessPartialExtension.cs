using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.CustomerDevelopment;
using FubuCore.Binding;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.View;

namespace ChpokkWeb.Infrastructure {
	public static class ModellessPartialExtension {
		public static string ModellessPartial<THandler>(this IFubuPage page, string methodName) {
			var factory = page.Get<IPartialFactory>();
			var writer = page.Get<IOutputWriter>();
			var graph = page.Get<BehaviorGraph>();
			var serviceArguments = page.Get<ServiceArguments>();
			var activityTracker = page.Get<ActivityTracker>();
			var logger = new Action<string>(msg => { activityTracker.Record("ModellessPartial for " + typeof(THandler).FullName + "." + methodName + ": " + msg); });
			return Partial<THandler>(methodName, factory, graph, serviceArguments, writer, logger);
		}

		public static string Partial<THandler>(string methodName, IPartialFactory partialFactory, BehaviorGraph graph, ServiceArguments serviceArguments, IOutputWriter writer, Action<string> logger) {
			logger("calling Partial");
			var thisChain =
				graph.Behaviors.FirstOrDefault(
					chain => {
						var actionCall = chain.FirstCall();
						return actionCall != null && actionCall.HandlerType == typeof(THandler) && actionCall.Method.Name == methodName;
					});
			logger("got thisChain");
			var actionBehavior = partialFactory.BuildBehavior(thisChain);
			logger("got actionBehavior");
			var output = writer.Record(() => {
				logger("invoking");
				actionBehavior.InvokePartial();
				logger("invoked");
			}).GetText();
			logger("got output");
			return output;
		}
	}
}