using System;
using ChpokkWeb.Features.ReadMe;

namespace ChpokkWeb.Features.Blog.Post {
	public class BlogPostModel: BaseContentModel {
		public string[] Categories { get; set; }
	}
}