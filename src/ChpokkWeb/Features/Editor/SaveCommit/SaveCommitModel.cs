namespace ChpokkWeb.Features.Editor.SaveCommit {
	public class SaveCommitModel {
		public bool DoCommit { get; set; }
		public string FilePath { get; set; }
		public string Content { get; set; }
		public string CommitMessage { get; set; }
	}
}