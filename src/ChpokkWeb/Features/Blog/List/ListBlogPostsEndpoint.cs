using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Blog.Post;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.Blog.List {
	public class ListBlogPostsEndpoint {
		private readonly IAppRootProvider _appRootProvider;
		private readonly IFileSystem _fileSystem;
		private readonly BlogPostParser _parser;
		public ListBlogPostsEndpoint(IAppRootProvider appRootProvider, IFileSystem fileSystem, BlogPostParser parser) {
			_appRootProvider = appRootProvider;
			_fileSystem = fileSystem;
			_parser = parser;
		}

		public ListBlogPostsModel DoIt() {
			var blogRoot = _appRootProvider.AppRoot.AppendPath("Blog");
			var postPaths = _fileSystem.FindFiles(blogRoot, FileSet.Shallow("*.md"));
			var postModels = from path in postPaths select _parser.LoadAndParse(path);
			return new ListBlogPostsModel { Posts = postModels.ToArray() };
		}
	}

	public class ListBlogPostsModel {
		public IEnumerable<BlogPostModel> Posts { get; set; } 
	}
}