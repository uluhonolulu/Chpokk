using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Files;

namespace ChpokkWeb.Features.Remotes.SaveCommit {
	public class SaveCommitInputModel : SaveFileInputModel {
		public string CommitMessage { get; set; }
	}
}