﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using ChpokkWeb.Features.Blog.Post;
using ChpokkWeb.Features.CustomerDevelopment;
using ChpokkWeb.Features.ReadMe;
using FubuCore;
using Tanka.Markdown;
using Tanka.Markdown.Html;

namespace ChpokkWeb.Features.Blog {
	public class BlogPostParser<TModel> where TModel:BaseContentModel, new() {
		private readonly MarkdownParser _parser = new MarkdownParser(false);
		private const string MetadataBoundary = "\r\n==\r\n";
		private const string DescriptionBoundary = "\r\n--\r\n";
		private readonly IFileSystem _fileSystem;
		private ActivityTracker _activityTracker;
		public BlogPostParser(IFileSystem fileSystem, ActivityTracker activityTracker) {
			_fileSystem = fileSystem;
			_activityTracker = activityTracker;
		}

		public TModel Parse(string source) {
			_activityTracker.Record("Parse start");
			var parts = source.Split(new[] {MetadataBoundary}, StringSplitOptions.None);
			var content = parts.Length > 1 ? parts[1].Replace(DescriptionBoundary, Environment.NewLine + Environment.NewLine) : string.Empty;
			var blogPostModel = new TModel { Content = ParseContent(content) };
			AddMetadata(blogPostModel, parts[0]);
			var description = parts.Length > 1 ? parts[1].Split(new[] {DescriptionBoundary}, StringSplitOptions.None)[0] : string.Empty;
			blogPostModel.Description = description;
			blogPostModel.HtmlDescription = ParseContent(description);
			try {
				return blogPostModel;
			}
			finally {
				_activityTracker.Record("Parse end");
			}
		}

		private void AddMetadata(TModel blogPostModel, string metadataSource) {
			var lines = metadataSource.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines) {
				var splitted = line.Split(new[] { ':' }, 2);
				var propertyName = splitted[0].Trim();
				var property = typeof (BlogPostModel).GetProperty(propertyName);
				if (property != null) {
					var stringValue = splitted[1].Trim();
					if (property.PropertyType.IsArray) {
						var propertyValue = (from s in stringValue.Split(',') select s.Trim()).ToArray();
						property.SetValue(blogPostModel, propertyValue);						
					}
					else {
						property.SetValue(blogPostModel, Convert.ChangeType(stringValue, property.PropertyType, CultureInfo.InvariantCulture));						
					}
	
				}
			}

		}

		private string ParseContent(string source) {
			var document = _parser.Parse(source);
			var renderer = new MarkdownHtmlRenderer();
			return renderer.Render(document);
		}

		public TModel LoadAndParse(string filePath) {
			var content = _fileSystem.ReadStringFromFile(filePath);
			var model = this.Parse(content);
			model.Slug = Path.GetFileNameWithoutExtension(filePath);
			return model;
		}
	}
}