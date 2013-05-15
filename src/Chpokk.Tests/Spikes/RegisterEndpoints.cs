using System;
using System.Collections.Generic;
using System.Text;
using CThru.BuiltInAspects;
using FubuMVC.Core.Registration;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb]
	public class RegisterEndpoints {
		[Test]
		public void TraceAllActionSourceCalls	() {
			var session = new TestSession();
			session.AddAspect(new TraceAspect(info => info.TargetInstance is ActionSource));
			session.Get("Main");
		}
	}
}
