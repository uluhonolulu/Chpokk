using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.ProjectManagement;
using ChpokkWeb.Features.RepositoryManagement;
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
				Data = new Dictionary<string, string> {{"Folder", filePath.ParentDirectory().PathRelativeTo(repositoryRoot)}},
				Type = "folder"
			};
			_fileSystem.ReadStringFromFile(filePath);
			var solutionFolder = filePath.ParentDirectory();
			var projectItems = _solutionParser.GetProjectItems(filePath).Select(item => CreateProjectItem(solutionFolder, item, repositoryRoot)).Where(item => item != null); 
			solutionItem.Children.AddRange(projectItems);

			return solutionItem;			
		}


		public RepositoryItem CreateProjectItem(string solutionFolder, ProjectItem projectItem, string repositoryRoot) {
			var projectFilePath = FileSystem.Combine(solutionFolder,
													 projectItem.Path);
			if (!_fileSystem.FileExists(projectFilePath)) {
				return null;
			}

			var projectRepositoryItem = new RepositoryItem()
			{
				Name = projectItem.Name,
				Type = "folder",
				PathRelativeToRepositoryRoot = projectFilePath.PathRelativeTo(repositoryRoot),
				Data = new Dictionary<string, string> { { "ProjectPath", projectFilePath.PathRelativeTo(repositoryRoot)}, {"Folder", projectFilePath.ParentDirectory().PathRelativeTo(repositoryRoot)} }
			};
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var projectFolderRelativeToRepositoryRoot = projectFilePath.ParentDirectory().PathRelativeTo(repositoryRoot);
			var fileItems = _projectParser.GetProjectFiles(projectFileContent);
			projectRepositoryItem.Children.AddRange(_converter.Convert(fileItems, projectFolderRelativeToRepositoryRoot));
			return projectRepositoryItem;
		}

		public void CreateEmptySolution(string solutionPath) {
			const string emptySolutionContent = @"Microsoft Visual Studio Solution File, Format Version 11.00
# Visual Studio 2010";
			_fileSystem.WriteStringToFile(solutionPath, emptySolutionContent);
		}
	}
}