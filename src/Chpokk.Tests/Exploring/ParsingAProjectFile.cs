using System;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class ParsingAProjectFile {
		[Test]
		public void Test() {
			const string projectFileContent = 
				@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Compile Include=""Class1.cs"" />
				  </ItemGroup>
				</Project>";
			var parser = new ProjectParser();
			var items = parser.GetProjectItems(projectFileContent);
			Assert.AreEqual(1, items.Count());
		}
	}
}
