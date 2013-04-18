using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.View;

namespace ChpokkWeb.Infrastructure {
	public static class ModellessPartialExtension {
		public static string RenderAction<THandler>(this IFubuPage page, string methodName) {
			var factory = page.Get<IPartialFactory>();
			var writer = page.Get<IOutputWriter>();
			return RenderAction<THandler>(methodName, writer, factory);
		}

		public static string RenderAction<THandler>(string methodName, IOutputWriter writer, IPartialFactory factory) {
			var actionCall = new ActionCall(typeof (THandler), typeof (THandler).GetMethod(methodName));
			return writer.Record(() => factory.BuildPartial(actionCall).InvokePartial()).GetText();
		}
	}
}