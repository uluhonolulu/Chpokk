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
		private ProjectParser _projectParser;
		private IFileSystem _fileSystem;
		public SolutionParser(ProjectParser projectParser, IFileSystem fileSystem) {
			_projectParser = projectParser;
			_fileSystem = fileSystem;
		}

		public void FillSolutionData(RepositoryItem solutionItem, string content) {
			solutionItem.Children.AddRange(this.GetProjectItems(content));
		}

		public IEnumerable<RepositoryItem> GetProjectItems(string content) {
			return content
				.Split(new[] {Environment.NewLine}, StringSplitOptions.None)
				.Select(line => projectLinePattern.Match(line))
				.Where(match => match.Success)
				.Select(match => CreateProjectItem(match));
		} 
		
		private RepositoryItem CreateProjectItem([NotNull] Match match) {
			var projectItem = new RepositoryItem {Name = match.Result("${Title}"), PathRelativeToRepositoryRoot = match.Result("${Location}"), Type = "folder"};
			var projectFilePath = FileSystem.Combine(@"F:\Projects\Fubu\Chpokk\src\ChpokkWeb\Repka",
			                                         match.Result("${Location}"));
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var fileItems = _projectParser.GetProjectItems(projectFileContent);
			projectItem.Children.AddRange(fileItems);
			return projectItem;
		}
	}
}