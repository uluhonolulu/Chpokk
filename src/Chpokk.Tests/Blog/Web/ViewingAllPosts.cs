using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;
using Shouldly;

namespace Chpokk.Tests.Blog {
	[TestFixture, RunOnWeb]
	public class ViewingAllPosts: WebQueryTest<OneBlogPostContext, WebResponse> {
		[Test]
		public void CanSeeTheList() {
			Assert.AreEqual("OK", Result.StatusDescription);
			Result.StatusDescription.ShouldBe("OK");
		}

		public override WebResponse Act() {
			return new TestSession().ProcessRequest(new WebRequest("blog/list"){ThrowOnError = false});
		}
	}
}
