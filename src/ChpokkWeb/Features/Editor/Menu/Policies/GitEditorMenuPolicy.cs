using System.IO;
using ChpokkWeb.Features.Remotes.Git;
using ChpokkWeb.Features.RepositoryManagement;

namespace ChpokkWeb.Features.Editor.Menu.Policies {

	public class GitEditorMenuPolicy : VersionControlEditorMenuPolicy {
		private readonly GitDetectionPolicy _detectionPolicy;
		public GitEditorMenuPolicy(GitDetectionPolicy detectionPolicy) {
			_detectionPolicy = detectionPolicy;
		}

		public override bool Matches(RepositoryInfo info, string approot) {
			return _detectionPolicy.Matches(info.Name);
		}


	}
}