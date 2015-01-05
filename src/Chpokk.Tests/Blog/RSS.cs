using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Xml;
using CThru;
using CThru.BuiltInAspects;
using ChpokkWeb.Features.Blog;
using ChpokkWeb.Features.Blog.List;
using ChpokkWeb.Features.Blog.Post;
using FubuMVC.Core.Http;
using FubuMVC.Media.Atom;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Arractas;
using Shouldly;
using System.Linq;

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
			return Context.Container.Get<ListBlogPostsEndpoint>().GetFeed(new[] {model});
			var httpRequest = Context.Container.Get<ICurrentHttpRequest>();
			var item = new SyndicationItem(model.Title, SyndicationContent.CreateXhtmlContent(model.Content), new Uri(httpRequest.ToFullUrl("/blog/post/" + model.Slug), UriKind.RelativeOrAbsolute), model.Slug, model.Date) 
			{ Title = new TextSyndicationContent(model.Title, TextSyndicationContentKind.Plaintext) , Summary = new TextSyndicationContent("summary", TextSyndicationContentKind.Html), PublishDate = model.Date, Content = SyndicationContent.CreatePlaintextContent(model.HtmlDescription) };
			var items = new[] {item};
			var feed = new SyndicationFeed("My Blog", "Bloggin bout Chpokk", new Uri(httpRequest.ToFullUrl("/blog/list") , UriKind.RelativeOrAbsolute), items);
			feed.LastUpdatedTime =
				items.Select(syndicationItem => syndicationItem.LastUpdatedTime).OrderByDescending(offset => offset).First();
			feed.Language = "en";
			item.Categories.Add(new SyndicationCategory("CodeProject"));

			var doc = new XmlDocument(){PreserveWhitespace = true};
			using (var writer = doc.CreateNavigator().AppendChild()) {
				feed.SaveAsRss20(writer);
				//writer.Flush();
			}

			return doc;
		}
	}
}
