using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ChpokkWeb.Infrastructure;
using System.Linq;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionParser {
		[NotNull] private static readonly Regex projectLinePattern = new Regex("Project\\(\"(?<ProjectGuid>.*)\"\\)\\s+=\\s+\"(?<Title>.*)\",\\s*\"(?<Location>.*)\",\\s*\"(?<Guid>.*)\"", RegexOptions.Compiled);

		public IEnumerable<ProjectItem> GetProjectItems(string content, string solutionPath) {
			return from match in projectLinePattern.Matches(content).Cast<Match>()
			       select CreateProjectItem(match);
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