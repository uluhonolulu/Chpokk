using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core;
using ICSharpCode.AvalonEdit.Highlighting;

namespace ChpokkWeb.Editor.Colorizer {
	public class ColorizerController {
		public string ToHtml(ColorizerInputModel input) {
			var highlightDefinition = HighlightingManager.Instance.GetDefinition("C#");
			var writer = new HtmlWriter();
			return writer.GenerateHtml(input.Code?? "", highlightDefinition);
		}
	}
}