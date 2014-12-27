using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Xml;
using CThru;
using CThru.BuiltInAspects;
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
			var item = new SyndicationItem("About Chpokk", "blah blah", new Uri("/blog/post", UriKind.RelativeOrAbsolute) , "id", DateTime.Parse("2014-12-08"));
			var feed = new SyndicationFeed("My Blog", "Bloggin bout chpokk", new Uri("/blog/list", UriKind.RelativeOrAbsolute), new[] {item});

			var doc = new XmlDocument();
			using (var writer = doc.CreateNavigator().AppendChild()) {
				feed.SaveAsRss20(writer);
				//writer.Flush();
			}

			return doc;
		}
	}
}
