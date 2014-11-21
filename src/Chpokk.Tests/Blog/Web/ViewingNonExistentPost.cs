using System;
using System.Collections.Generic;
using System.Text;
using Chpokk.Tests.Infrastructure;
using Gallio.Framework;
using Ivonna.Framework;
using Ivonna.Framework.Generic;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Blog.Web {
	[TestFixture, RunOnWeb]
	public class ViewingNonExistentPost : WebQueryTest<SimpleConfiguredContext, WebResponse> {
		[Test]
		public void Returns404() {
			Console.WriteLine(Result.StatusDescription);
			Assert.AreEqual(404, Result.Status);
		}

		public override WebResponse Act() {
			var url = "blog/post/IDoNotExist";
			return new TestSession().ProcessRequest(new WebRequest(url) { ThrowOnError = false });
		}
	}
}
