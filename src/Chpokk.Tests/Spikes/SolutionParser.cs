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
	}
}
