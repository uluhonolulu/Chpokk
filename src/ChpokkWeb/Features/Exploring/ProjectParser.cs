using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Project;
using Microsoft.Build.Construction;
using ChpokkWeb.Infrastructure;

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

		public IEnumerable<FileProjectItem> GetProjectFiles(string projectFileContent) {
			XmlNamespaceManager xmlNamespaceManager;
			var doc = LoadXml(projectFileContent, out xmlNamespaceManager);
			var nodes = GetIncludes(new[] { "Compile", "Content", "Resource", "None" }, doc, xmlNamespaceManager);
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

		private IEnumerable<XmlNode> GetIncludes(IEnumerable<string> nodeNames, XmlDocument doc, XmlNamespaceManager xmlNamespaceManager) {
			return nodeNames.SelectMany(nodeName => GetIncludes(nodeName, doc, xmlNamespaceManager));
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

		public ProjectRootElement CreateProject(string outputType, SupportedLanguage language) {
			var rootElement = ProjectRootElement.Create();
			var targetImport =
				@"$(MSBuildToolsPath)\Microsoft.{0}.targets".ToFormat(language == SupportedLanguage.CSharp ? "CSharp" : "VisualBasic");
			rootElement.AddImport(targetImport);
			rootElement.AddProperty("OutputType", outputType);
			return rootElement;
		}

		public void AddProjectToSolution(string name, string solutionPath, SupportedLanguage language) {
			AddProjectToSolution(name, solutionPath, language.GetProjectGuid(), language.GetProjectExtension());
		} 

		public void AddProjectToSolution(string name, string solutionPath, string projectTypeGuid, string projectFileExtension) {
			var solutionContent = _fileSystem.ReadStringFromFile(solutionPath);
			solutionContent += Environment.NewLine;
			solutionContent += @"Project(""{1}"") = ""{0}"", ""{0}\{0}{3}"", ""{2}""
EndProject".ToFormat(name, projectTypeGuid, Guid.NewGuid(), projectFileExtension);
			_fileSystem.WriteStringToFile(solutionPath, solutionContent);
		}

		public void CreateItem(string projectFilePath, string fileName, string fileContent) {
			var project = ProjectRootElement.Open(projectFilePath);
			var extension = Path.GetExtension(fileName);
			var buildAction = "Content";
			if (extension == ".cs" || extension == ".vb") {
				buildAction = "Compile";
			}
			project.AddItem(buildAction, fileName);
			project.Save();
			//ProjectCollection.GlobalProjectCollection.UnloadProject(project);

			var filePath = projectFilePath.ParentDirectory().AppendPath(fileName);
			_fileSystem.WriteStringToFile(filePath, fileContent);
		}

		public void AddReference(ProjectRootElement projectRoot, string assemblyNameOrPath) {
			if (Path.IsPathRooted(assemblyNameOrPath)) {
				var assemblyName = Path.GetFileNameWithoutExtension(assemblyNameOrPath);
				var referenceElement = projectRoot.AddItem("Reference", assemblyName);
				referenceElement.AddMetadata("HintPath", assemblyNameOrPath.PathRelativeTo(projectRoot.DirectoryPath));
			}
			else {
				projectRoot.AddItem("Reference", assemblyNameOrPath);
			}
			
		}
	}

}