using System;
using System.Collections.Generic;
using System.Text;
using Chpokk.Tests.Infrastructure;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;
using Shouldly;

namespace Chpokk.Tests.Retrieving {
	[TestFixture, RunOnWeb]
	public class ZipSpike: WebQueryTest<SimpleConfiguredContext, Byte[]> {
		[Test]
		public void Test() {
			Result.ShouldBe(new byte[]{0x66});
		}

		public override byte[] Act() {
			var session = new TestSession();
			var response = session.Get("remotes/downloadzip/download"); 
			return response.Body;
		}
	}
}
