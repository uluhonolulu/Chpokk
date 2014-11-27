using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;
using FubuCore;

namespace ChpokkWeb.Features.Blog.Post {
	public class BlogPostEndpoint {
		private readonly IAppRootProvider _rootProvider;
		private readonly IFileSystem _fileSystem;
		private BlogPostParser _parser;
		public BlogPostEndpoint(IAppRootProvider rootProvider, IFileSystem fileSystem, BlogPostParser parser) {
			_rootProvider = rootProvider;
			_fileSystem = fileSystem;
			_parser = parser;
		}

		public BlogPostModel DoIt(BlogPostInputModel model) {
			var filePath = _rootProvider.AppRoot.AppendPath("Blog", model.Slug + ".md");
			//Console.WriteLine(filePath);
			if (_fileSystem.FileExists(filePath)) {
				return _parser.LoadAndParse(filePath);
			}
			throw new HttpException(404, "Post not found");
		}
	}

	[UrlPattern("/blog/{Slug}")]
	public class BlogPostInputModel {
		[RouteInput("")]
		public string Slug { get; set; }
	}

	public class BlogPostModel {
		public string Title { get; set; }
		public string Description { get; set; }
		public string Content { get; set; }
		public string Slug { get; set; }
	}
}