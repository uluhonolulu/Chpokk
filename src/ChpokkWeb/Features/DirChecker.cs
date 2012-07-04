using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using FubuCore;

namespace ChpokkWeb.Features {
	public class DirCheckerController {
		private IFileSystem _fileSystem;
		public DirCheckerController(IFileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public string Gimme(FileContentInputModel model) {
			return _fileSystem.ChildDirectoriesFor(FileSystem.Combine(model.PhysicalApplicationPath, model.RelativePath)).Join(Environment.NewLine);
			return _fileSystem.FindFiles(FileSystem.Combine(model.PhysicalApplicationPath, "bin"), FileSet.Everything()).Join(Environment.NewLine); 
		}
	}
}