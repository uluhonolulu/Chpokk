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
			solutionItem.Children.Add(new RepositoryItem());
			return solutionItem;
		}

		static Regex projectLinePattern = new Regex("Project\\(\"(?<ProjectGuid>.*)\"\\)\\s+=\\s+\"(?<Title>.*)\",\\s*\"(?<Location>.*)\",\\s*\"(?<Guid>.*)\"", RegexOptions.Compiled);

				//        Match match = projectLinePattern.Match(line);
				//if (match.Success) {
				//    string projectGuid  = match.Result("${ProjectGuid}");
				//    string title        = match.Result("${Title}");
				//    string location     = match.Result("${Location}");
				//    string guid         = match.Result("${Guid}");
	}
}