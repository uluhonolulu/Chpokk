using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Editor.Intellisense {
	public class IntelInputModel {
		public string Text { get; set; }
		public char NewChar { get; set; }
		public int Position { get; set; }
		public string Message { get; set; }
	}
}