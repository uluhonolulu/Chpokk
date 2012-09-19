namespace ChpokkWeb.Features.Editor.Intellisense {
	public class IntelInputModel {
		public string Text { get; set; }
		public char NewChar { get; set; }
		public int Position { get; set; }
		public string Message { get; set; }
		public string PhysicalApplicationPath { get; set; }		
		public string RepositoryName { get; set; }
		public string ProjectPath { get; set; }
	}
}