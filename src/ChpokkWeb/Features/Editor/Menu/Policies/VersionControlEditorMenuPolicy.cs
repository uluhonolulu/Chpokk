using System.Collections.Generic;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.Editor.Menu.Policies {
	public abstract class VersionControlEditorMenuPolicy : IEditorMenuPolicy {
		public abstract bool Matches(string repositoryPath);

		public IEnumerable<MenuItemToken> GetMenuItems() {
			yield return new MenuItemToken() { Text = "Save and Commit", Key = "saveCommit", MenuItemState = MenuItemState.Available };
		}
	}
}