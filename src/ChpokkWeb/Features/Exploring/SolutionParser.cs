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
			return content
				.Split(new[] {Environment.NewLine}, StringSplitOptions.None)
				.Select(line => projectLinePattern.Match(line))
				.Where(match => match.Success)
				.Select(match => CreateProjectItem(match, item => PostProcessProjectItem(solutionPath.ParentDirectory(), match.Result("${Location}"), item)));
		}

		private RepositoryItem CreateProjectItem([NotNull] Match match, [NotNull] Action<RepositoryItem> projectLoader) {
			var projectTitle = match.Result("${Title}");
			var projectPath = match.Result("${Location}");
			var projectItem = new RepositoryItem {Name = projectTitle, PathRelativeToRepositoryRoot = projectPath, Type = "folder"};
			projectLoader(projectItem);
			return projectItem;
		}

		private void PostProcessProjectItem(string solutionFolder, string projectPath, RepositoryItem projectItem) {
			var projectFilePath = FileSystem.Combine(solutionFolder,
			                                         projectPath);
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var fileItems = _projectParser.GetProjectItems(projectFileContent, "");
			projectItem.Children.AddRange(fileItems);
		}
	}
}