﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Editor.Menu;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.RepositoryManagement.FileSystem {
	public class StandardEditorMenuPolicy : IEditorMenuPolicy {
		public bool Matches(string repositoryPath) {
			return true;
		}

		public IEnumerable<MenuItemToken> GetMenuItems() {
			yield return new MenuItemToken(){Text = "Save", Key = "save", MenuItemState = MenuItemState.Available};
		}
	}
}