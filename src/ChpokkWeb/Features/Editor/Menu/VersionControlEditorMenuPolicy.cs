using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.Editor.Menu {
	public abstract class VersionControlEditorMenuPolicy : IEditorMenuPolicy {
		public abstract bool Matches(RepositoryInfo info, string approot);

		public IEnumerable<MenuItemToken> GetMenuItems() {
			yield return new MenuItemToken() { Text = "Save and Commit", Key = "saveCommit", MenuItemState = MenuItemState.Available };
		}
	}
}