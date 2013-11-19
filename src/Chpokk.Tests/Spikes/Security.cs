using System;
using System.Collections.Generic;
using System.Text;
using CThru.BuiltInAspects;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb]
	public class Security {
		[Test]
		public void LetsSeeWhichSecurityRulesAreApplied() {
			var session = new TestSession();
			session.AddAspect(new TraceAspect(info => info.TypeName.StartsWith("FubuMVC.Core.Security") || info.TypeName.Contains("Authorization")));
			session.Get("");
			Console.WriteLine("=========================================================");
			session.AddAspect(new TraceAspect(info => info.TargetInstance is Exception, 10));
			session.Get("editor/keepalive");
		}
	}
}
