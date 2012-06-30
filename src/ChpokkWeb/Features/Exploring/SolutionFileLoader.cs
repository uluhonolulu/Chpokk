using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionFileLoader {
		[NotNull]
		private readonly SolutionParser _solutionParser;
		[NotNull]
		private readonly IFileSystem _fileSystem;
		private readonly ProjectParser _projectParser;

		public SolutionFileLoader(SolutionParser solutionParser, IFileSystem fileSystem, ProjectParser projectParser) {
			_solutionParser = solutionParser;
			_fileSystem = fileSystem;
			_projectParser = projectParser;
		}

		public RepositoryItem CreateSolutionItem(string repositoryRoot, string filePath) {
			var solutionItem = new RepositoryItem {
				Name = filePath.GetFileNameUniversal(),
				PathRelativeToRepositoryRoot = filePath.PathRelativeTo(repositoryRoot),
				Type = "folder"
			};
			var content = _fileSystem.ReadStringFromFile(filePath);
			var solutionFolder = filePath.ParentDirectory();
			var projectItems = _solutionParser.GetProjectItems(content, filePath).Select(item => CreateProjectItem(solutionFolder, item)); 
			solutionItem.Children.AddRange(projectItems);

			return solutionItem;			
		}


		public RepositoryItem CreateProjectItem(string solutionFolder, ProjectItem projectItem) {
			var projectFilePath = FileSystem.Combine(solutionFolder,
													 projectItem.Path);
			var projectRepositoryItem = new RepositoryItem() {
				Name = projectItem.Name,
				PathRelativeToRepositoryRoot = projectFilePath,
				Type = "folder"
			};
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var fileItems = _projectParser.GetCompiledFiles(projectFileContent).Select(item => new RepositoryItem());
			projectRepositoryItem.Children.AddRange(fileItems);
			return projectRepositoryItem;
		}

	}
}