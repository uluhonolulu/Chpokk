using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionExplorer {
		private readonly IFileSystem _fileSystem;
		public SolutionExplorer(IFileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public IEnumerable<string> GetSolutionFiles(string repositoryRoot) {
			return _fileSystem.FindFiles(repositoryRoot, new FileSet { Include = "*.sln" });
		}
	}
}