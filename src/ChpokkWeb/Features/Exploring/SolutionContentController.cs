using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionContentController {
		[NotNull]
		private readonly RepositoryManager _repositoryManager;

		[NotNull]
		private IFileSystem _fileSystem;

		public SolutionContentController(RepositoryManager repositoryManager, IFileSystem fileSystem) {
			_repositoryManager = repositoryManager;
			_fileSystem = fileSystem;
		}

		public IEnumerable<RepositoryItem> GetSolutions(SolutionExplorerInputModel model) {
			var info = _repositoryManager.GetRepositoryInfo(model.Name);
			var folder = FileSystem.Combine(model.PhysicalApplicationPath, info.Path);
			Console.WriteLine(folder);
			var files = _fileSystem.FindFiles(folder, new FileSet(){ Include = "*.sln" });// 
			return files.Select(filePath => new RepositoryItem(){Name = Path.GetFileName(filePath)});//filePath, 
		}
	}
}