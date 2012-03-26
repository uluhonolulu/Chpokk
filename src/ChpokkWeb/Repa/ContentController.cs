using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Urls;

namespace ChpokkWeb.Repa {
	public class ContentController {
		private IUrlRegistry _registry;
		public ContentController(IUrlRegistry registry) {
			_registry = registry;
		}

		[UrlPattern("Project/{Name}")]
		public string GetFileList(RepositoryInputModel model) {
			return _registry.TemplateFor(model, "GET");
			//return _registry.UrlFor<ContentController>(c => c.GetFileList(new RepositoryInputModel(){Name = "stuff"}));
			//return "wow";
			return model.Name;
		}
	}
}