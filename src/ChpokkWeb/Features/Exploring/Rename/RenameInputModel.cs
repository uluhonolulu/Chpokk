namespace ChpokkWeb.Features.Exploring.Rename {
	public class RenameInputModel:BaseFileInputModel {
		public string NewFileName { get; set; }
		public string ItemType { get; set; }
		public string SolutionPath { get; set; }
		public string ProjectPath { get; set; }
	}
}