using System.Collections.Generic;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.Editor.Menu {
	public interface IEditorMenuPolicy {
		bool Matches(RepositoryInfo info, string approot);
		IEnumerable<MenuItemToken> GetMenuItems();
	}
}
