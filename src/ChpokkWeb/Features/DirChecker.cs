using System;
using System.Collections.Generic;
using FubuCore;
using System.Linq;

namespace ChpokkWeb.Features {
	public class DirCheckerController {
		private IFileSystem _fileSystem;
		public DirCheckerController(IFileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public string Gimme(GimmeModel model) {
			var directory = FileSystem.Combine(model.PhysicalApplicationPath, model.RelativePath);
			return _fileSystem.ChildDirectoriesFor(directory).Union(_fileSystem.FindFiles(directory, FileSet.Everything())).Join(Environment.NewLine);
			return _fileSystem.FindFiles(FileSystem.Combine(model.PhysicalApplicationPath, "bin"), FileSet.Everything()).Join(Environment.NewLine); 
		}

		public class GimmeModel {
			public string PhysicalApplicationPath { get; set; }
			public string RelativePath { get; set; }
		}

		public string GetPath() {
			return Environment.GetEnvironmentVariable("PATH");
		}
	}
}