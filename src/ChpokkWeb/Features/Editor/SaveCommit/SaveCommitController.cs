using System;
using ChpokkWeb.Features.Exploring;
using FubuCore;

namespace ChpokkWeb.Features.Editor.SaveCommit {
	public class SaveCommitController {
		private readonly FileSystem _fileSystem;
		private RepositoryManager _manager;
		public SaveCommitController(FileSystem fileSystem, RepositoryManager manager) {
			_fileSystem = fileSystem;
			_manager = manager;
		}

		public void Save(SaveCommitModel saveCommitModel) {
			var filePath = _manager.GetPhysicalFilePath(saveCommitModel);
			Console.WriteLine("Now writing " + saveCommitModel.Content + " to " + filePath);
			_fileSystem.WriteStringToFile(filePath, saveCommitModel.Content);
		}
	}
}