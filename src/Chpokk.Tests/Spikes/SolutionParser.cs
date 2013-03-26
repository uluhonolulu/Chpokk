using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			var source = File.ReadAllLines(@"D:\Projects\Chpokk\src\Chpokk.sln");
			var projectNames = from line in source
			                   where projectLinePattern.Match(line).Success
			                   select projectLinePattern.Match(line).Result("${Title}");
			Assert.AreElementsEqualIgnoringOrder(new[] {"Chpokk.Tests", "ChpokkWeb"}, projectNames);
		}

		[Test]
		public void CanSeeClassLib() {
			//var projectLinePattern = new Regex("Project\\(\"(?<ProjectGuid>.*)\"\\)\\s+=\\s+\"(?<Title>.*)\",\\s*\"(?<Location>.*)\",\\s*", RegexOptions.Compiled | RegexOptions.Multiline);
			var source = @"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""ConsoleApplication1"", ""ConsoleApplication1\ConsoleApplication1.csproj"", ""{7F5E6663-10AD-4671-80E6-8095EE4BC6F9}""
				EndProject
				Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""ClassLibrary1"", ""ClassLibrary1\ClassLibrary1.csproj"", ""{6FEA811B-AABB-465F-932F-D0FB930AAAB5}""
				EndProject";
			var sourceLines =
				source.Split(new[]{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			var projectNames =
				sourceLines.Where(line => projectLinePattern.Match(line).Success)
				      .Select(line => {
					                      Console.WriteLine(line);
										  Console.WriteLine(projectLinePattern.Matches(line).Count);
					                      return projectLinePattern.Match(line).Result("${Title}"); });
			var lineOfProject =
				@"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""ClassLibrary1"", ""ClassLibrary1\ClassLibrary1.csproj"", ""{6FEA811B-AABB-465F-932F-D0FB930AAAB5}""";
			Assert.IsTrue(projectLinePattern.Match(lineOfProject).Success);
			;
			projectNames = from match in projectLinePattern.Matches(source).Cast<Match>() select match.Result("${Title}");
			Assert.AreElementsEqualIgnoringOrder(new[] { "ConsoleApplication1", "ClassLibrary1" }, projectNames);


		}
	}
}
