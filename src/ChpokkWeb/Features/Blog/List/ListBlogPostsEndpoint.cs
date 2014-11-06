using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.Blog.List {
	public class ListBlogPostsEndpoint {
		public ListBlogPostsModel DoIt() {
			return new ListBlogPostsModel();
		}
	}

	public class ListBlogPostsModel {}
}