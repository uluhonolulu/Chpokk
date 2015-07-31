using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.Editor.Menu {
	public class EditorMenuModel {
		public IEnumerable<MenuItem> MenuItems { get; set; }
	}
}