using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core;

namespace ChpokkWeb.Features.Blog.Post {
	public class BlogPostEndpoint {
		public BlogPostModel DoIt(BlogPostInputModel model) {
			return new BlogPostModel();
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