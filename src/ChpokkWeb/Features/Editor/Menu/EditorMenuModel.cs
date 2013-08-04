using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.Editor.Menu {
	public class EditorMenuModel {
		public IEnumerable<MenuItemToken> MenuItems { get; set; }
	}
}