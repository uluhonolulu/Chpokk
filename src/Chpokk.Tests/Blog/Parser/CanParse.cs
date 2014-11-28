using System;
using System.Globalization;
using System.Web;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Blog;
using ChpokkWeb.Features.Blog.Post;
using MbUnit.Framework;
using Shouldly;

namespace Chpokk.Tests.Blog.Parser {
	[TestFixture]
	public class CanParse : BaseQueryTest<SimpleConfiguredContext, BlogPostModel> {
		[Test]
		public void ParsedContentShouldContainContent() {
			//Console.WriteLine(HttpUtility.HtmlEncode(Result.Content));
			Result.Content.ShouldContain("<p>more stuff</p>");
		}

		[Test]
		public void ParsedContentShouldContainDescription() {
			Result.Content.ShouldContain("<p>description</p>");
		}

		[Test]
		public void ParsedDescriptionShouldMatchSourceDescription() {
			Result.Description.ShouldContain("<p>description</p>");
		}

		[Test]
		public void TitleShouldMatchThatFromTheHeaderPart() {
			Result.Title.ShouldBe("title");
		}

		[Test]
		public void DateShouldMatchThatFromTheSource() {
			Result.Date.ShouldBe(DateTime.Parse("12/5/2014", CultureInfo.InvariantCulture));
		}

	public override BlogPostModel Act() {
			var sampleBlogPost = @"
Title: title
Date: 2014-12-05
Tags: good, bad
==
description
--
more stuff";
			return Context.Container.Get<BlogPostParser>().Parse(sampleBlogPost);
		}
	}
}
