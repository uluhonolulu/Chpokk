using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using System.Linq;
using FubuCore;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionParser {
		private IFileSystem _fileSystem;
		[NotNull] private static readonly Regex projectLinePattern = new Regex("Project\\(\"(?<ProjectGuid>.*)\"\\)\\s+=\\s+\"(?<Title>.*)\",\\s*\"(?<Location>.*)\",\\s*\"(?<Guid>.*)\"", RegexOptions.Compiled);
		public SolutionParser(IFileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public IEnumerable<ProjectItem> ParseSolutionContent(string content) {
			return from match in projectLinePattern.Matches(content).Cast<Match>()
			       select CreateProjectItem(match);
		}

		public IEnumerable<ProjectItem> GetProjectItems(string solutionPath) {
			return ParseSolutionContent(_fileSystem.ReadStringFromFile(solutionPath));
		}

		private ProjectItem CreateProjectItem(Match match) {
			var projectTitle = match.Result("${Title}");
			var projectPath = match.Result("${Location}");
			var projectItem = new ProjectItem {Name = projectTitle, Path = projectPath};
			return projectItem;
		}
	}

	public class ProjectItem {
		public string Name { get; set; }
		public string Path { get; set; }
	}
}