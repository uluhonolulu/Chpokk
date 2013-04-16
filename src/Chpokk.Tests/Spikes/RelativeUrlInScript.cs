using System;
using System.Collections.Generic;
using System.Text;
using CThru.BuiltInAspects;
using FubuMVC.Core.Http;
using FubuMVC.Core.Urls;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb]
	public class RelativeUrlInScript {
		[Test]
		public void Test() {
			var session = new TestSession();
			session.AddAspect(new TraceResultAspect(info => info.TargetInstance is ICurrentHttpRequest));
			session.Get("/_content/janrain.js");
		}
	}
}
