using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;

namespace ChpokkWeb.Features.Files {
	public class Savior {
		private readonly FileSystem _fileSystem;
		private readonly RepositoryManager _manager;
		public Savior(FileSystem fileSystem, RepositoryManager manager) {
			_fileSystem = fileSystem;
			_manager = manager;
		}

		public void SaveFile(SaveFileInputModel model) {
			var filePath = _manager.GetPhysicalFilePath(model);
			_fileSystem.WriteStringToFile(filePath, model.Content);			
		}
	}
}