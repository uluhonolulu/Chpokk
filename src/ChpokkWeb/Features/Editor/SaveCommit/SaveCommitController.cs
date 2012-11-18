using FubuCore;

namespace ChpokkWeb.Features.Editor.SaveCommit {
	public class SaveCommitController {
		private readonly FileSystem _fileSystem;
		public SaveCommitController(FileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public void Save(SaveCommitModel saveCommitModel) {
			_fileSystem.WriteStringToFile(saveCommitModel.FilePath, saveCommitModel.Content);
		}
	}
}