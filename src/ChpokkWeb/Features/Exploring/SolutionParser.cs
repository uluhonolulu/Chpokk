using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ChpokkWeb.Infrastructure;
using System.Linq;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionParser {
		[NotNull] private static readonly Regex projectLinePattern = new Regex("Project\\(\"(?<ProjectGuid>.*)\"\\)\\s+=\\s+\"(?<Title>.*)\",\\s*\"(?<Location>.*)\",\\s*\"(?<Guid>.*)\"", RegexOptions.Compiled);


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
			return new RepositoryItem{Name = match.Result("${Title}"), PathRelativeToRepositoryRoot = match.Result("${Location}"), Type = "folder"};
		}
	}
}