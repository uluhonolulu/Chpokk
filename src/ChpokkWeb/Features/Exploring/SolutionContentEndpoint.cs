using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Features.Storage;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core;
using HtmlTags;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionContentEndpoint {
		[NotNull] private readonly RepositoryManager _repositoryManager;

		[NotNull] private readonly SolutionFileLoader _solutionFileLoader;

		private readonly SolutionExplorer _solutionExplorer;
		private readonly RestoreSynchronizer _restoreSynchronizer;

		public SolutionContentEndpoint([NotNull]RepositoryManager repositoryManager, [NotNull] SolutionFileLoader solutionFileLoader, SolutionExplorer solutionExplorer, RestoreSynchronizer restoreSynchronizer) {
			_repositoryManager = repositoryManager;
			_solutionFileLoader = solutionFileLoader;
			_solutionExplorer = solutionExplorer;
			_restoreSynchronizer = restoreSynchronizer;
		}

		[JsonEndpoint]
		public SolutionExplorerModel GetSolutions([NotNull]SolutionExplorerInputModel model) {
			if (HttpContext.Current != null) HttpContext.Current.Server.ScriptTimeout = 3600;//HACK: to make sure everything's been downloaded
			var items = GetSolutionRepositoryItems(model.RepositoryName);
			return new SolutionExplorerModel {Items = items.ToArray()};
		}

		private IEnumerable<RepositoryItem> GetSolutionRepositoryItems(string repositoryName) {
			var repositoryRoot = _repositoryManager.GetAbsoluteRepositoryPath(repositoryName);
			_restoreSynchronizer.WaitTillRestored(repositoryRoot);
			var files = _solutionExplorer.GetSolutionFiles(repositoryRoot);
			var items =
				files.Select(
					filePath =>
					_solutionFileLoader.CreateSolutionItem(filePath, repositoryRoot));
			return items;
		}

		[JsonEndpoint]
		public SolutionExplorerModel GetSolutionFolders(SolutionFolderExplorerInputModel model) {
			var items = GetSolutionRepositoryItems(model.RepositoryName).ToArray();
			foreach (var solutionItem in items) {
				solutionItem.Type = "solution";
				solutionItem.PathRelativeToRepositoryRoot = null;
				foreach (var projectItem in solutionItem.Children) {
					projectItem.Type = "project";
					projectItem.PathRelativeToRepositoryRoot = projectItem.Data["ProjectPath"].ParentDirectory(); //no need?
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