using ICSharpCode.AvalonEdit.Highlighting;

namespace ChpokkWeb.Features.Editor.Colorizer {
	public class ColorizerController {
		public string ToHtml(ColorizerInputModel input) {
			var highlightDefinition = HighlightingManager.Instance.GetDefinition("C#");
			var writer = new HtmlWriter();
			return writer.GenerateHtml(input.Code?? "", highlightDefinition);
		}
	}
}