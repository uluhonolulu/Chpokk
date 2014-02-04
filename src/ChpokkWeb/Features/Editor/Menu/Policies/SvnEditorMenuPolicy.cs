using System.IO;
using ChpokkWeb.Features.RepositoryManagement;

namespace ChpokkWeb.Features.Editor.Menu.Policies {
	public class SvnEditorMenuPolicy: VersionControlEditorMenuPolicy {
		private readonly RepositoryManager _repositoryManager;
		public SvnEditorMenuPolicy(RepositoryManager repositoryManager) {
			_repositoryManager = repositoryManager;
		}

		public override bool Matches(RepositoryInfo info, string approot) {
			var path = _repositoryManager.NewGetAbsolutePathFor(info.Name, ".svn");
			return Directory.Exists(path);
		}
	}
}