using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using ICSharpCode.SharpDevelop.Project;
using Microsoft.Build.Construction;

namespace ChpokkWeb.Features.Exploring {
	public class ProjectParser {
		private readonly IFileSystem _fileSystem;
		public ProjectParser(IFileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public IEnumerable<FileProjectItem> GetCompiledFiles(string projectFileContent) {
			XmlNamespaceManager xmlNamespaceManager;
			var doc = LoadXml(projectFileContent, out xmlNamespaceManager);
			var nodes = GetIncludes("Compile", doc, xmlNamespaceManager);
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
			var assemblyElements = GetElements("Reference", doc, xmlNamespaceManager);
			var assemblyReferences = assemblyElements.Select(element => CreateReferenceItem(element));
			var projectNodes = GetIncludes("ProjectReference", doc, xmlNamespaceManager);
			var projectReferences = projectNodes.Select(node => CreateProjectReference(node));
			return
				assemblyReferences.Concat(projectReferences);
		}

		private ProjectReferenceProjectItem CreateProjectReference(XmlNode projectNode) {
			return new ProjectReferenceProjectItem(new UnknownProject(@"C:\something", String.Empty), new MissingProject(@"C:\something", String.Empty)) {Include = projectNode.Value};
		}

		private ReferenceProjectItem CreateReferenceItem(XmlElement referenceElement) {
			var hint =
				(from child in referenceElement.ChildNodes.Cast<XmlNode>() where child.Name == "HintPath" select child).FirstOrDefault();
			var hintPath = hint != null ? hint.InnerText : null;
			return new ReferenceProjectItem(null, referenceElement.GetAttribute("Include")){HintPath = hintPath};
		}

		private IEnumerable<XmlNode> GetIncludes(string nodeName, XmlDocument doc, XmlNamespaceManager xmlNamespaceManager) {
			var elements = GetElements(nodeName, doc, xmlNamespaceManager);
			return elements.Select(element => element.GetAttributeNode("Include"));
		}

		private IEnumerable<XmlElement> GetElements(string nodeName, XmlDocument doc, XmlNamespaceManager xmlNamespaceManager) {
			var xpath = "//ms:{0}".ToFormat(nodeName);
			return doc.SelectNodes(xpath, xmlNamespaceManager).Cast<XmlElement>();
		}

		private XmlDocument LoadXml(string projectFileContent, out XmlNamespaceManager xmlNamespaceManager) {
			var doc = new XmlDocument();
			doc.LoadXml(projectFileContent);
			xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);
			xmlNamespaceManager.AddNamespace("ms", "http://schemas.microsoft.com/developer/msbuild/2003");
			return doc;
		}

		public void CreateProjectFile(string outputType, string projectPath) {
			var rootElement = ProjectRootElement.Create();
			rootElement.AddImport(@"$(MSBuildToolsPath)\Microsoft.CSharp.targets");
			rootElement.AddProperty("OutputType", outputType);
			rootElement.Save(projectPath);
		}

		public void AddProjectToSolution(string name, string solutionPath) {
			var projectTypeGuid = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
			var solutionContent = _fileSystem.ReadStringFromFile(solutionPath);
			solutionContent += Environment.NewLine;
			solutionContent += @"Project(""{{{1}}}"") = ""{0}"", ""{0}\{0}.csproj"", ""{{{2}}}""
EndProject".ToFormat(name, projectTypeGuid, Guid.NewGuid());
			_fileSystem.WriteStringToFile(solutionPath, solutionContent);
		}
	}

}