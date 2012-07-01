using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionFileLoader {
		[NotNull]
		private readonly SolutionParser _solutionParser;
		[NotNull]
		private readonly IFileSystem _fileSystem;
		private readonly ProjectParser _projectParser;
		private readonly FileItemToProjectItemConverter _converter;

		public SolutionFileLoader(SolutionParser solutionParser, IFileSystem fileSystem, ProjectParser projectParser, FileItemToProjectItemConverter converter) {
			_solutionParser = solutionParser;
			_fileSystem = fileSystem;
			_projectParser = projectParser;
			_converter = converter;
		}

		public RepositoryItem CreateSolutionItem(string repositoryRoot, string filePath) {
			var solutionItem = new RepositoryItem {
				Name = filePath.GetFileNameUniversal(),
				PathRelativeToRepositoryRoot = filePath.PathRelativeTo(repositoryRoot),
				Type = "folder"
			};
			var content = _fileSystem.ReadStringFromFile(filePath);
			var solutionFolder = filePath.ParentDirectory();
			var projectItems = _solutionParser.GetProjectItems(content, filePath).Select(item => CreateProjectItem(solutionFolder, item, repositoryRoot)); 
			solutionItem.Children.AddRange(projectItems);

			return solutionItem;			
		}


		public RepositoryItem CreateProjectItem(string solutionFolder, ProjectItem projectItem, string repositoryRoot) {
			var projectFilePath = FileSystem.Combine(solutionFolder,
													 projectItem.Path);
			var projectRepositoryItem = new RepositoryItem() {
				Name = projectItem.Name,
				Type = "folder"
			};
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var projectFolderRelativeToRepositoryRoot = projectFilePath.ParentDirectory().PathRelativeTo(repositoryRoot);
			var fileItems = _projectParser.GetCompiledFiles(projectFileContent);
			projectRepositoryItem.Children.AddRange(_converter.Convert(fileItems, projectFolderRelativeToRepositoryRoot));
			return projectRepositoryItem;
		}

	}
}