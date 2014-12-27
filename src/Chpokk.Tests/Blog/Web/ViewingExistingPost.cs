using System;
using System.IO;
using System.Web;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Blog;
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
			Console.WriteLine(HttpUtility.HtmlEncode(Result.BodyAsString));
			Assert.Contains(Result.BodyAsString, Context.BLOGPOST_CONTENT);
		}

		public override WebResponse Act() {
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is BlogPostParser<BlogPostModel>));
			var url = "blog/post/" + Path.GetFileNameWithoutExtension(Context.FILENAME);
			return new TestSession().ProcessRequest(new WebRequest(url) { ThrowOnError = false });
		}
	}
}
