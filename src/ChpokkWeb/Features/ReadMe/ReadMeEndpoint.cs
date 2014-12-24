using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Blog;
using ChpokkWeb.Features.Blog.Post;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core;

namespace ChpokkWeb.Features.ReadMe {
	public class ReadMeEndpoint {
		private readonly IAppRootProvider _rootProvider;
		private readonly IFileSystem _fileSystem;
		private readonly BlogPostParser<ReadMeModel> _parser;
		public ReadMeEndpoint(IAppRootProvider rootProvider, IFileSystem fileSystem, BlogPostParser<ReadMeModel> parser) {
			_rootProvider = rootProvider;
			_fileSystem = fileSystem;
			_parser = parser;
		}

		public ReadMeModel DoIt(ReadMeInputModel model) {
			var filePath = _rootProvider.AppRoot.AppendPath("ReadMe", model.Name + ".md");
			if (_fileSystem.FileExists(filePath)) {
				return _parser.LoadAndParse(filePath);
			}
			throw new HttpException(404, "Post not found");
		}
	}

	public class ReadMeInputModel {
		[RouteInput]
		public string Name { get; set; }
	}

	public class ReadMeModel: BaseContentModel {}

	public class BaseContentModel {
		public string Title { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public string HtmlDescription { get; set; }
		public string Content { get; set; }
		public string Slug { get; set; }
	}
}