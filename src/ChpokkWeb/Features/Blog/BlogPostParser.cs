﻿using System;
using System.IO;
using ChpokkWeb.Features.Blog.Post;
using FubuCore;
using Tanka.Markdown;
using Tanka.Markdown.Html;

namespace Chpokk.Tests.Blog.Parser {
	public class BlogPostParser {
		private readonly MarkdownParser _parser = new MarkdownParser(false);
		private const string MetadataBoundary = "\r\n==\r\n";
		private const string DescriptionBoundary = "\r\n--\r\n";
		private readonly IFileSystem _fileSystem;
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