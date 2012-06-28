using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionContentController {
		[NotNull]
		private readonly RepositoryManager _repositoryManager;

		[NotNull]
		private readonly IFileSystem _fileSystem;

		[NotNull] private readonly SolutionParser _solutionParser;

		public SolutionContentController([NotNull]RepositoryManager repositoryManager, [NotNull]IFileSystem fileSystem, [NotNull
		                                                                                                                ] SolutionParser solutionParser) {
			_repositoryManager = repositoryManager;
			_fileSystem = fileSystem;
			_solutionParser = solutionParser;
		}

		[JsonEndpoint]
		public SolutionExplorerModel GetSolutions([NotNull]SolutionExplorerInputModel model) {
			var info = _repositoryManager.GetRepositoryInfo(model.Name);
			var folder = FileSystem.Combine(model.PhysicalApplicationPath, info.Path);
			var files = _fileSystem.FindFiles(folder, new FileSet { Include = "*.sln" });
			var items =
				files.Select(
					filePath =>
					CreateSolutionItem(folder, filePath));
			return new SolutionExplorerModel {Items = items.ToArray()};
		}

		private RepositoryItem CreateSolutionItem(string folder, string filePath) {
			var solutionItem = new RepositoryItem
			                     {
			                     	Name = Path.GetFileName(filePath), PathRelativeToRepositoryRoot = filePath.PathRelativeTo(folder), Type = "folder"
			                     };
			var content = _fileSystem.ReadStringFromFile(filePath);
			_solutionParser.FillSolutionData(solutionItem, content);
			return solutionItem;
		}
	}
}