using System;
using System.Web;
using Arractas;
using Chpokk.Tests.Infrastructure;
using MbUnit.Framework;
using Tanka.Markdown;
using Tanka.Markdown.Html;

namespace Chpokk.Tests.Blog.Parser {
	[TestFixture]
	public class CanParse: BaseQueryTest<SimpleConfiguredContext, BlogPostModel> {
		[Test]
		public void Test() {
			Console.WriteLine(HttpUtility.HtmlEncode(Result.Content));
		}

		public override BlogPostModel Act() {
			return Context.Container.Get<BlogPostParser>().Parse("header  \n=");
		}
	}

	public class BlogPostParser {
		private readonly MarkdownParser _parser = new MarkdownParser(false);
		//public BlogPostParser(MarkdownParser parser) {
		//	_parser = parser;
		//}

		public BlogPostModel Parse(string source) {
			var document = _parser.Parse(source);
			var renderer = new MarkdownHtmlRenderer();
			var content = renderer.Render(document);
			return new BlogPostModel {Content = content};
		}
	}

	public class BlogPostModel {
		public string Content { get; set; }
	}
}
