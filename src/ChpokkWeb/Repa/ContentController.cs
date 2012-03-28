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
			var repositoryRoot = Path.Combine(model.PhysicalApplicationPath, RepositoryInfo.Path);
			foreach (var file in Directory.GetFiles(repositoryRoot)) {
				var fileName = Path.GetFileName(file);
				var relativePath = file.Substring(repositoryRoot.Length);
				fileList.Add("li")
					.Data("path", relativePath)
					.Data("name", fileName)
					.Data("type", "file")
					.Text(fileName);
			}
			return fileList;
		}
	}
}