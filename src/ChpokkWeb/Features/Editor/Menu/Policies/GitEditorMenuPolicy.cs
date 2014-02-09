using System.IO;
using ChpokkWeb.Features.Remotes.Git;

namespace ChpokkWeb.Features.Editor.Menu.Policies {

	public class GitEditorMenuPolicy : VersionControlEditorMenuPolicy {
		private readonly GitDetectionPolicy _detectionPolicy;

		public GitEditorMenuPolicy(GitDetectionPolicy detectionPolicy) {
			_detectionPolicy = detectionPolicy;
		}

		public override bool Matches(string repositoryPath) {
			return _detectionPolicy.Matches(repositoryPath);
		}


	}
}