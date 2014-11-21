using System.IO;
using Chpokk.Tests.Infrastructure;
using Ivonna.Framework;
using Ivonna.Framework.Generic;
using MbUnit.Framework;

namespace Chpokk.Tests.Blog.Web {
	[TestFixture, RunOnWeb]
	public class ViewingExistingPost : WebQueryTest<OneBlogPostContext, WebResponse> {
		[Test]
		public void CanSeeThePost() {
			Assert.AreEqual("OK", Result.StatusDescription);
		}

		[Test, DependsOn("CanSeeThePost")]
		public void CanSeeTheContent() {
			Assert.Contains(Result.BodyAsString, Context.BLOGPOST_CONTENT);
		}

		public override WebResponse Act() {
			var url = "blog/post/" + Path.GetFileNameWithoutExtension(Context.FILENAME);
			return new TestSession().ProcessRequest(new WebRequest(url) { ThrowOnError = false });
		}
	}
}
