using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FubuCore;
using ICSharpCode.SharpDevelop.Project;

namespace ChpokkWeb.Features.Exploring {
	public class ProjectParser {
		private readonly IFileSystem _fileSystem;
		public ProjectParser(IFileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public IEnumerable<FileProjectItem> GetCompiledFiles(string projectFileContent) {
			XmlNamespaceManager xmlNamespaceManager;
			var doc = LoadXml(projectFileContent, out xmlNamespaceManager);
			var nodes = GetNodes("Compile", doc, xmlNamespaceManager);
			var filePaths = nodes.Select(node => node.Value);
			return from path in filePaths select new FileProjectItem(null, ItemType.Compile, path);
		}

		public IEnumerable<string> GetFullPathsForCompiledFilesFromProjectFile(string projectFilePath) {
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var projectFolder = projectFilePath.ParentDirectory();
			var compiledFiles = this.GetCompiledFiles(projectFileContent);
			return from fileItem in compiledFiles select FileSystem.Combine(projectFolder, fileItem.Include); 
			
		}

		public IEnumerable<ReferenceProjectItem> GetReferences(string projectFileContent) {
			XmlNamespaceManager xmlNamespaceManager;
			var doc = LoadXml(projectFileContent, out xmlNamespaceManager);
			var assemblyNodes = GetNodes("Reference", doc, xmlNamespaceManager);
			var assemblyReferences = assemblyNodes.Select(node => new ReferenceProjectItem(null, node.Value));
			var projectNodes = GetNodes("ProjectReference", doc, xmlNamespaceManager);
			var projectReferences = projectNodes.Select(node => new ProjectReferenceProjectItem(new UnknownProject(".", string.Empty), new MissingProject(node.Value, string.Empty)){Include = node.Value});
			return
				assemblyReferences.Concat(projectReferences);
		}

		private IEnumerable<XmlNode> GetNodes(string nodeName, XmlDocument doc, XmlNamespaceManager xmlNamespaceManager) {
			var xpath = "//ms:{0}/@Include".ToFormat(nodeName);
			var nodes = doc.SelectNodes(xpath, xmlNamespaceManager).Cast<XmlNode>();
			return nodes;
		}

		private XmlDocument LoadXml(string projectFileContent, out XmlNamespaceManager xmlNamespaceManager) {
			var doc = new XmlDocument();
			doc.LoadXml(projectFileContent);
			xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);
			xmlNamespaceManager.AddNamespace("ms", "http://schemas.microsoft.com/developer/msbuild/2003");
			return doc;
		}
	}

}