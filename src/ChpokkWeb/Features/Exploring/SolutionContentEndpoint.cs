using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core;
using HtmlTags;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionContentEndpoint {
		[NotNull] private readonly RepositoryManager _repositoryManager;

		[NotNull] private readonly IFileSystem _fileSystem;

		[NotNull] private readonly SolutionFileLoader _solutionFileLoader;

		public SolutionContentEndpoint([NotNull]RepositoryManager repositoryManager, [NotNull]IFileSystem fileSystem, [NotNull
		                                                                                                                ] SolutionParser solutionParser, [NotNull] SolutionFileLoader solutionFileLoader) {
			_repositoryManager = repositoryManager;
			_fileSystem = fileSystem;
			_solutionFileLoader = solutionFileLoader;
		}

		[JsonEndpoint]
		public SolutionExplorerModel GetSolutions([NotNull]SolutionExplorerInputModel model) {
			var items = GetSolutionRepositoryItems(model.RepositoryName, model.PhysicalApplicationPath);
			if (!items.Any()) {
				throw new ApplicationException("No solution file found in the source. Please upload your files together with the solution file.");
			}
			return new SolutionExplorerModel {Items = items.ToArray()};
		}

		private IEnumerable<RepositoryItem> GetSolutionRepositoryItems(string repositoryName, string physicalApplicationPath) {
			var info = _repositoryManager.GetRepositoryInfo(repositoryName);
			var folder = FileSystem.Combine(physicalApplicationPath, info.Path);
			var files = _fileSystem.FindFiles(folder, new FileSet {Include = "*.sln"});
			var items =
				files.Select(
					filePath =>
					_solutionFileLoader.CreateSolutionItem(folder, filePath));
			return items;
		}

		[JsonEndpoint]
		public SolutionExplorerModel GetSolutionFolders(SolutionFolderExplorerInputModel model) {
			var items = GetSolutionRepositoryItems(model.RepositoryName, model.PhysicalApplicationPath).ToArray();
			foreach (var solutionItem in items) {
				solutionItem.Type = "solution";
				solutionItem.PathRelativeToRepositoryRoot = null;
				foreach (var projectItem in solutionItem.Children) {
					projectItem.Type = "project";
					projectItem.PathRelativeToRepositoryRoot = projectItem.Data["ProjectPath"].ParentDirectory();
					RemoveLeaves(projectItem);
				}
			}
			return new SolutionExplorerModel{Items = items.ToArray()};
		}

		private void RemoveLeaves(RepositoryItem item) {
			item.Children = item.Children.Where(childItem => childItem.Type == "folder").ToList();
			foreach (var child in item.Children) {
				RemoveLeaves(child);
			}
		}
	}

	public class SolutionFolderExplorerInputModel: BaseRepositoryInputModel {}
}