using System;
using System.Collections.Generic;
using System.Text;
using CThru.BuiltInAspects;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;
using Shouldly;

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb]
	public class FontProblem {
		[Test]
		public void CanDownloadAFont() {
			var session = new TestSession();
			session.AddAspect(new TraceAspect(info => info.TargetInstance is Exception, 10));
			session.AddAspect(new TraceAspect(info => info.TypeName.Contains("Asset")));
			var response = session.Get("_content/styles/fonts/glyphicons-halflings-regular.woff");
			response.Status.ShouldBe(200);
		}
	}
}
