using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Blog.List;
using Ivonna.Framework;
using MbUnit.Framework;
using Ivonna.Framework.Generic;
using Shouldly;
using System.Linq;

namespace Chpokk.Tests.Blog.Web {
	[TestFixture, RunOnWeb]
	public class ViewingAllPostsWeb: WebQueryTest<OneBlogPostContext, WebResponse> {
		[Test]
		public void CanSeeTheList() {
			Assert.AreEqual("OK", Result.StatusDescription);
			Result.StatusDescription.ShouldBe("OK");
		}

		public override WebResponse Act() {
			return new TestSession().ProcessRequest(new WebRequest("blog/list"){ThrowOnError = false});
		}
	}

	public class ViewingAllPosts: BaseQueryTest<OneBlogPostContext, ListBlogPostsModel> {

		[Test]
		public void ShouldbeOnePost() {
			Result.Posts.Count().ShouldBe(1);
		}

		public override ListBlogPostsModel Act() {
			return Context.Container.Get<ListBlogPostsEndpoint>().DoIt();
		}
	}
}
