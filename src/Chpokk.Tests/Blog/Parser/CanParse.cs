using Arractas;
using Chpokk.Tests.Infrastructure;
using MbUnit.Framework;

namespace Chpokk.Tests.Blog.Parser {
	[TestFixture]
	public class CanParse: BaseQueryTest<SimpleConfiguredContext, BlogPostModel> {
		[Test]
		public void Test() {
			//
			// TODO: Add test logic here
			//
		}

		public override BlogPostModel Act() {
			return Context.Container.Get<BlogPostParser>().Parse("");
		}
	}

	public class BlogPostParser {
		public BlogPostModel Parse(string source) {
			return null;
		}
	}

	public class BlogPostModel {}
}
