using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Blog.Post;

namespace ChpokkWeb.Features.Blog.List {
	public class ListBlogPostsEndpoint {
		public ListBlogPostsModel DoIt() {
			return new ListBlogPostsModel{Posts = new [] {new BlogPostModel()}};
		}
	}

	public class ListBlogPostsModel {
		public IEnumerable<BlogPostModel> Posts { get; set; } 
	}
}