using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CThru;
using CThru.BuiltInAspects;

namespace Chpokk.Tests.Infrastructure {
	class TimingTraceAspect: TraceAspect {
		public DateTime Started { get; set; }
		public TimingTraceAspect(Predicate<InterceptInfo> shouldIntercept) : base(shouldIntercept) {
			Started = DateTime.Now;
		}
		public TimingTraceAspect(Predicate<InterceptInfo> shouldIntercept, string logPath) : base(shouldIntercept, logPath) {
			Started = DateTime.Now;
		}
		protected override string GetMessage(DuringCallbackEventArgs e) {
			var interval = DateTime.Now.Subtract(Started);
			var milliseconds = interval.Seconds*1000 + interval.Milliseconds;
			return milliseconds.ToString() + ", " + DateTime.Now + ". " + base.GetMessage(e);
		}
	}
}
