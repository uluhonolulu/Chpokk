using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionContentController {
		[NotNull]
		private readonly RepositoryManager _repositoryManager;

		[NotNull]
		private IFileSystem _fileSystem;

		public SolutionContentController([NotNull]RepositoryManager repositoryManager, [NotNull]IFileSystem fileSystem) {
			_repositoryManager = repositoryManager;
			_fileSystem = fileSystem;
		}

		[JsonEndpoint]
		public SolutionExplorerModel GetSolutions([NotNull]SolutionExplorerInputModel model) {
			var info = _repositoryManager.GetRepositoryInfo(model.Name);
			var folder = FileSystem.Combine(model.PhysicalApplicationPath, info.Path);
			var files = _fileSystem.FindFiles(folder, new FileSet { Include = "*.sln" });
			var items = files.Select(filePath => new RepositoryItem {Name = Path.GetFileName(filePath)});
			return new SolutionExplorerModel {Items = items.ToArray()};
		}
	}
}