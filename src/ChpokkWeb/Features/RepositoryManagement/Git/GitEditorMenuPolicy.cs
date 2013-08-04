using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Editor.Menu;
using FubuCore;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.RepositoryManagement.Git {
	public class GitEditorMenuPolicy: IEditorMenuPolicy {
		public bool Matches(RepositoryInfo info, string approot) {
			//var path = FileSystem.Combine(approot, info.Path, ".git");
			//return Directory.Exists(path);
			return true;
		}

		public IEnumerable<MenuItemToken> GetMenuItems() {
			yield return new MenuItemToken(){Text = "Save and Commit", Key = "saveCommit", MenuItemState = MenuItemState.Available};
		}
	}
}