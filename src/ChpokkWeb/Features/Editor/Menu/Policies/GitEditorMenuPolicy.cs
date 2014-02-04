using System.IO;
using ChpokkWeb.Features.RepositoryManagement;

namespace ChpokkWeb.Features.Editor.Menu.Policies {

	public class GitEditorMenuPolicy : VersionControlEditorMenuPolicy {
		private readonly RepositoryManager _repositoryManager;
		public GitEditorMenuPolicy(RepositoryManager repositoryManager) {
			_repositoryManager = repositoryManager;
		}

		public override bool Matches(RepositoryInfo info, string approot) {
			var path = _repositoryManager.NewGetAbsolutePathFor(info.Name, ".git");
			return Directory.Exists(path);
		}


	}
}