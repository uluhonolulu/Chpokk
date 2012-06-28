using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ChpokkWeb.Features.Exploring;

namespace Chpokk.Tests.Exploring {
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