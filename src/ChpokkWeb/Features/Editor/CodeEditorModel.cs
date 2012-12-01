using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Editor {
	public class CodeEditorModel {
		[NotNull]
		public string Content { get; set; }
		[NotNull]
		public string RepositoryName { get; set; }
		[NotNull]
		public string ProjectPath { get; set; }
		[NotNull]
		public string PathRelativeToRepositoryRoot { get; set; }
	}
}