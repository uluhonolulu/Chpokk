using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using HtmlTags;

namespace ChpokkWeb.Repa {
	public class ContentController {
		private IUrlRegistry _registry;
		public ContentController(IUrlRegistry registry) {
			_registry = registry;
		}

		

		//[UrlPattern("Project/{Name}")]
		public HtmlTag GetFileList(RepositoryFileContentModel model) {
			var fileList = new HtmlTag("ul");
			var folder = Path.Combine(model.PhysicalApplicationPath, RepositoryInfo.Path);
			foreach (var file in Directory.GetFiles(folder)) {
				var fileName = Path.GetFileName(file);
				fileList.Add("li").Data("path", file).Data("name", fileName).Text(fileName);
			}
			return fileList;
		}
	}
}