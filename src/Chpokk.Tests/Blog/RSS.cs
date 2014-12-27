using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Xml;
using CThru;
using CThru.BuiltInAspects;
using ChpokkWeb.Features.Blog;
using ChpokkWeb.Features.Blog.Post;
using FubuMVC.Media.Atom;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Arractas;
using Shouldly;

namespace Chpokk.Tests.Blog {
	[TestFixture]
	public class RSS : BaseQueryTest<OneBlogPostContext, XmlDocument> {
		[Test]
		public void ShouldContainTheBlogPost() {
			Result.DocumentElement.SelectNodes("/rss/channel/item").Count.ShouldBe(1);
		}

		public override XmlDocument Act() {
			var path = Context.BlogPostPath;
			var model = Context.Container.Get<BlogPostParser<BlogPostModel>>().LoadAndParse(path);
			var item = new SyndicationItem(model.Title, SyndicationContent.CreateXhtmlContent(), new Uri("/blog/post/" + model.Slug, UriKind.RelativeOrAbsolute) , model.Slug, model.Date){Content = SyndicationContent.CreatePlaintextContent(model.HtmlDescription)};
			var feed = new SyndicationFeed("My Blog", "Bloggin bout chpokk", new Uri("/blog/list", UriKind.RelativeOrAbsolute), new[] {item});

			var doc = new XmlDocument(){PreserveWhitespace = true};
			using (var writer = doc.CreateNavigator().AppendChild()) {
				feed.SaveAsRss20(writer);
				//writer.Flush();
			}

			return doc;
		}
	}
}
