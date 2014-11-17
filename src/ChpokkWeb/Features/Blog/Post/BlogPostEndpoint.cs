using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.Blog.Post {
	public class BlogPostEndpoint {
		public BlogPostModel DoIt(BlogPostInputModel model) {
			return new BlogPostModel();
		}
	}

	public class BlogPostInputModel {
		public string Slug { get; set; }
	}

	public class BlogPostModel {
		public string Title { get; set; }
		public string Description { get; set; }
		public string Content { get; set; }
	}
}