using System.Collections.Generic;
using ChpokkWeb.Infrastructure;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.Editor.Menu.Policies {
	public abstract class VersionControlEditorMenuPolicy : IEditorMenuPolicy {
		public abstract bool Matches(string repositoryPath);

		public IEnumerable<MenuItem> GetMenuItems(string repositoryPath) {
			yield return new MenuItem() { Caption = "Save and Commit", Id = "saveCommit" };
		}
	}
}