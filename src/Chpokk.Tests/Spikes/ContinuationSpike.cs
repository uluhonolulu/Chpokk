using System;
using System.Collections.Generic;
using System.Text;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Infrastructure;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;
using FubuCore;

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb]
	public class ContinuationSpike {
		[Test]
		public void Test() {
			var session = new TestSession();
			session.Get("Main");
			//session.AddAspect(new TraceAspect(info => info.TypeName.Contains("Continuation")));
			//session.AddAspect(new TraceResultAspect(info => info.MethodName == "IsRedirectable"));
			//session.AddAspect(new TraceAspect(info => info.TargetInstance is IActionBehavior));
			//session.AddAspect(new TraceResultAspect(info => info.MethodName == "get_InsideBehavior")); 
			//session.AddAspect(new TraceAspect(info => info.MethodName == "Partial"));
			session.AddAspect(new TraceClassAspect(info => info.TargetInstance is ContinuationHandler));
			//session.AddAspect(new TraceClassAspect(info => info.MethodName.Contains("Invoke")));
			session.Get("Main");
		}
	}

	public class TraceClassAspect: TraceAspect {

		protected override string GetMessage(DuringCallbackEventArgs e) {
			if (e.TargetInstance is IActionBehavior) {
				return "-{" + e.TargetInstance.As<IActionBehavior>().ToTheString() + "} " + base.GetMessage(e);
			}
			return "[" + e.TargetInstance.GetType().Name + "] " + base.GetMessage(e);
		}

		public TraceClassAspect(Predicate<InterceptInfo> shouldIntercept) : base(shouldIntercept) {}
		public TraceClassAspect(Predicate<InterceptInfo> shouldIntercept, int depth) : base(shouldIntercept, depth) {}
	}

	public static class BehaviorExtensions {
		public static string ToTheString(this IActionBehavior behavior) {
			var result = behavior.ToString();
			var basic = behavior as BasicBehavior;
			if (basic != null) {
				var inner = basic.InsideBehavior;
				if (inner != null) {
					result += " --> " + inner.ToTheString();
				}
			}
			var wrappingBehavior = behavior as WrappingBehavior;
			if (wrappingBehavior != null) {
				var inner = wrappingBehavior.Inner;
				if (inner != null) {
					result += " --> " + inner.ToTheString();
				}
			}
			return result;
		}
	}

	[TestFixture, RunOnWeb]
	public class WhichChains: WebQueryTest<SimpleConfiguredContext, String> {
		
		[Test]
		public void showMe() {
			var session = new TestSession();
			session.Get("Main");
			var graph = Context.Container.Get<BehaviorGraph>();
			foreach (var behavior in graph.Behaviors) {
				Console.WriteLine(behavior.ToString());
				Console.WriteLine(behavior.Top.ToString());
				Console.WriteLine();
			}
		}
		
		public override string Act() {
			return null;
		}
	}
}
