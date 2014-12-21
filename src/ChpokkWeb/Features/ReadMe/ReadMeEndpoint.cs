using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Blog.Post;
using FubuMVC.Core;

namespace ChpokkWeb.Features.ReadMe {
	public class ReadMeEndpoint {
		public ReadMeModel DoIt(ReadMeInputModel model) {
			return new ReadMeModel(){Content = "about me", Title = model.Name};
		}
	}

	//[UrlPattern("/readme/{Name}")]
	public class ReadMeInputModel {
		[RouteInput]
		public string Name { get; set; }
	}

	public class ReadMeModel: BlogPostModel {}
}