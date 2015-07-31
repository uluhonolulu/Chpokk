using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Editor.Menu;
using ChpokkWeb.Infrastructure;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.RepositoryManagement.FileSystem {
	public class StandardEditorMenuPolicy : IEditorMenuPolicy {
		public bool Matches(string repositoryPath) {
			return true;
		}

		public IEnumerable<MenuItem> GetMenuItems(string repositoryPath) {
			yield return new MenuItem() { Caption = "Save", Id = "save" };
		}
	}
}