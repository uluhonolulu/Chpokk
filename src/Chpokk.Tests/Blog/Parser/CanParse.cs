using System;
using System.IO;
using System.Web;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Blog.Post;
using FubuCore;
using MbUnit.Framework;
using Tanka.Markdown;
using Tanka.Markdown.Html;
using Shouldly;

namespace Chpokk.Tests.Blog.Parser {
	[TestFixture]
	public class CanParse: BaseQueryTest<SimpleConfiguredContext, BlogPostModel> {
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

		public override BlogPostModel Act() {
			var sampleBlogPost = @"
Title: title
Date: 05.12.2014
Tags: good, bad
==
description
--
more stuff";
			return Context.Container.Get<BlogPostParser>().Parse(sampleBlogPost);
		}
	}

	public class BlogPostParser {
		private readonly MarkdownParser _parser = new MarkdownParser(false);
		private const string MetadataBoundary = "\r\n==\r\n";
		private const string DescriptionBoundary = "\r\n--\r\n";
		private IFileSystem _fileSystem;
		public BlogPostParser(IFileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public BlogPostModel Parse(string source) {
			var parts = source.Split(new[] {MetadataBoundary}, StringSplitOptions.None);
			var content = parts.Length > 1 ? parts[1].Replace(DescriptionBoundary, Environment.NewLine + Environment.NewLine) : string.Empty;
			var blogPostModel = new BlogPostModel {Content = ParseContent(content)};
			AddMetadata(blogPostModel, parts[0]);
			var description = parts.Length > 1 ? parts[1].Split(new[] {DescriptionBoundary}, StringSplitOptions.None)[0] : string.Empty;
			blogPostModel.Description = ParseContent(description);
			return blogPostModel;
		}

		private void AddMetadata(BlogPostModel blogPostModel, string metadataSource) {
			var lines = metadataSource.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines) {
				var splitted = line.Split(':');
				var propertyName = splitted[0].Trim();
				var property = typeof (BlogPostModel).GetProperty(propertyName);
				if (property != null) {
					property.SetValue(blogPostModel, splitted[1].Trim());		
				}
			}

		}

		private string ParseContent(string source) {
			var document = _parser.Parse(source);
			var renderer = new MarkdownHtmlRenderer();
			return renderer.Render(document);
		}

		public BlogPostModel LoadAndParse(string filePath) {
			var content = _fileSystem.ReadStringFromFile(filePath);
			var model = this.Parse(content);
			model.Slug = Path.GetFileNameWithoutExtension(filePath);
			return model;
		}
	}


}
