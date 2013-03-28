using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.ProjectManagement;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionFileLoader {
		[NotNull]
		private readonly SolutionParser _solutionParser;
		[NotNull]
		private readonly IFileSystem _fileSystem;
		[NotNull]
		private readonly ProjectParser _projectParser;
		[NotNull]
		private readonly FileItemToProjectItemConverter _converter;
		[NotNull]
		private readonly ProjectFactory _projectFactory;

		public SolutionFileLoader(SolutionParser solutionParser, IFileSystem fileSystem, ProjectParser projectParser, FileItemToProjectItemConverter converter, ProjectFactory projectFactory) {
			_solutionParser = solutionParser;
			_fileSystem = fileSystem;
			_projectParser = projectParser;
			_converter = converter;
			_projectFactory = projectFactory;
		}

		public RepositoryItem CreateSolutionItem(string repositoryRoot, string filePath) {
			var solutionItem = new RepositoryItem {
				Name = filePath.GetFileNameUniversal(),
				PathRelativeToRepositoryRoot = filePath.PathRelativeTo(repositoryRoot),
				Type = "folder"
			};
			var content = _fileSystem.ReadStringFromFile(filePath);
			var solutionFolder = filePath.ParentDirectory();
			var projectItems = _solutionParser.GetProjectItems(content, filePath).Select(item => CreateProjectItem(solutionFolder, item, repositoryRoot)).Where(item => item != null); 
			solutionItem.Children.AddRange(projectItems);

			return solutionItem;			
		}


		public RepositoryItem CreateProjectItem(string solutionFolder, ProjectItem projectItem, string repositoryRoot) {
			var projectFilePath = FileSystem.Combine(solutionFolder,
													 projectItem.Path);
			if (!_fileSystem.FileExists(projectFilePath)) {
				return null;
			}
			// touch the project so that it is compiled
			_projectFactory.GetProjectData(projectFilePath);
			var projectRepositoryItem = new RepositoryItem()
			{
				Name = projectItem.Name,
				Type = "folder",
				Data = new Dictionary<string, string> { { "ProjectPath", projectFilePath.PathRelativeTo(repositoryRoot)} }
			};
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var projectFolderRelativeToRepositoryRoot = projectFilePath.ParentDirectory().PathRelativeTo(repositoryRoot);
			var fileItems = _projectParser.GetCompiledFiles(projectFileContent);
			projectRepositoryItem.Children.AddRange(_converter.Convert(fileItems, projectFolderRelativeToRepositoryRoot));
			return projectRepositoryItem;
		}

	}
}