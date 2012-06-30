using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ChpokkWeb.Infrastructure;
using System.Linq;
using FubuCore;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionParser {
		[NotNull] private static readonly Regex projectLinePattern = new Regex("Project\\(\"(?<ProjectGuid>.*)\"\\)\\s+=\\s+\"(?<Title>.*)\",\\s*\"(?<Location>.*)\",\\s*\"(?<Guid>.*)\"", RegexOptions.Compiled);
		private readonly ProjectParser _projectParser;
		private readonly IFileSystem _fileSystem;
		public SolutionParser(ProjectParser projectParser, IFileSystem fileSystem) {
			_projectParser = projectParser;
			_fileSystem = fileSystem;
		}

		public void FillSolutionData(RepositoryItem solutionItem, string content, string solutionPath) {
			solutionItem.Children.AddRange(this.GetProjectItems(content, solutionPath));
		}

		public IEnumerable<RepositoryItem> GetProjectItems(string content, string solutionPath) {
			var solutionFolder = solutionPath.ParentDirectory();
			return content
				.Split(new[] {Environment.NewLine}, StringSplitOptions.None)
				.Select(line => projectLinePattern.Match(line))
				.Where(match => match.Success)
				.Select(match => CreateProjectItem(match, solutionFolder));
		}

		private RepositoryItem CreateProjectItem([NotNull] Match match, string solutionFolder) {
			var projectTitle = match.Result("${Title}");
			var projectPath = match.Result("${Location}");
			var projectFilePath = FileSystem.Combine(solutionFolder,
			                                         projectPath);
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			return CreateProjectItem(projectFileContent, projectTitle, projectPath);
		}

		private RepositoryItem CreateProjectItem(string projectFileContent, string projectTitle, string projectPath) {
			var projectItem = new RepositoryItem {Name = projectTitle, PathRelativeToRepositoryRoot = projectPath, Type = "folder"};
			var fileItems = _projectParser.GetProjectItems(projectFileContent, "");
			projectItem.Children.AddRange(fileItems);
			return projectItem;
		}
	}
}