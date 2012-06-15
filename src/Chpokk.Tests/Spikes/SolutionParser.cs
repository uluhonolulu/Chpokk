using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class SolutionParser {
		static readonly Regex globalSectionPattern = new Regex("\\s*GlobalSection\\((?<Name>.*)\\)\\s*=\\s*(?<Type>.*)", RegexOptions.Compiled);
		static readonly Regex projectLinePattern = new Regex("Project\\(\"(?<ProjectGuid>.*)\"\\)\\s+=\\s+\"(?<Title>.*)\",\\s*\"(?<Location>.*)\",\\s*\"(?<Guid>.*)\"", RegexOptions.Compiled);
		[Test]
		public void CanLoadTheSolution() {
			var source = File.ReadAllLines(@"F:\Projects\Fubu\Chpokk\src\Chpokk.sln");
			foreach (var line in source) {
				var match = projectLinePattern.Match(line);
				if (match.Success) {
					Console.WriteLine(match.Result("${Title}"));
				}
			}
			// see ICSharpCode.SharpDevelop.Project and ICSharpCode.SharpDevelop.Project.Services.ParserService namespaces
			//var solution = Solution.Load(@"F:\Projects\Fubu\Chpokk\src\Chpokk.sln");
			//foreach (var project in solution.Projects) {
			//    if (project != null) Console.WriteLine(project.Name);
			//}
		}
	}
}
