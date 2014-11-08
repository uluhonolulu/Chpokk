using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Chpokk.Tests.Infrastructure;
using Gallio.Framework;
using Ivonna.Framework;
using Ivonna.Framework.Generic;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Blog {
	[TestFixture]
	public class ViewingExistingPost : WebQueryTest<OneBlogPostContext, WebResponse> {
		[Test]
		public void CanSeeThePost() {
			Assert.AreEqual("OK", Result.StatusDescription);
		}

		public override WebResponse Act() {
			var url = "blog/post/" + Path.GetFileNameWithoutExtension(Context.FILENAME);
			return new TestSession().ProcessRequest(new WebRequest(url) { ThrowOnError = false });
		}
	}
}
