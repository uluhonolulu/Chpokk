using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ChpokkWeb.Features.Exploring;
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

	public class ProjectParser {
		public IEnumerable<RepositoryItem> GetProjectItems(string projectFileContent) {
			var doc = new XmlDocument();
			doc.LoadXml(projectFileContent);
			var xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);
			xmlNamespaceManager.AddNamespace("ms", "http://schemas.microsoft.com/developer/msbuild/2003");
			var filePaths = doc.SelectNodes("//ms:Compile/@Include", xmlNamespaceManager).Cast<XmlNode>().Select(node => node.Value);
			return from path in filePaths select new RepositoryItem {Name = path};
		}
	}
}
