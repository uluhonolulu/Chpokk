using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.Editor.Menu.Policies {
	public class WebPublishEditorMenuPolicy :IEditorMenuPolicy{
		public bool Matches(string repositoryPath) {
			var webConfigPaths = Directory.EnumerateFiles(repositoryPath, "web.config", SearchOption.AllDirectories);
			return webConfigPaths.Any();
		}

		public IEnumerable<MenuItemToken> GetMenuItems() {
			yield return new MenuItemToken() { Text = "Preview your Web", Key = "publishWeb", MenuItemState = MenuItemState.Available };
		}
	}
}