using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Blog.List;
using ChpokkWeb.Infrastructure;
using Ivonna.Framework;
using MbUnit.Framework;
using Ivonna.Framework.Generic;
using Shouldly;
using System.Linq;
using FubuCore;

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
		public void ShouldbeAsManyPostsAsMarkdownFiles() {
			var blogFolder = Context.Container.Get<IAppRootProvider>().AppRoot.AppendPath("Blog");
			var fileCount =
				Context.Container.Get<FileSystem>()
				       .FindFiles(blogFolder, new FileSet() {Include = "*.md", DeepSearch = true})
				       .Count();
			Result.Posts.Count().ShouldBe(fileCount);
		}

		public override ListBlogPostsModel Act() {
			return Context.Container.Get<ListBlogPostsEndpoint>().DoIt();
		}
	}
}
