using ChpokkWeb.Features.Exploring;

namespace ChpokkWeb.Features.Remotes.SaveCommit {
	public class SaveCommitInputModel : BaseFileInputModel {
		public string Content { get; set; }
		public bool DoCommit { get; set; }
		public string CommitMessage { get; set; }
	}
}