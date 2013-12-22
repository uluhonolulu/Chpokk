using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Project;
using ChpokkWeb.Infrastructure;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace ChpokkWeb.Features.Exploring {
	public class ProjectParser {
		private readonly IFileSystem _fileSystem;
		public ProjectParser(IFileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public IEnumerable<FileProjectItem> GetCompiledFiles(string projectFileContent) {
			return GetIncludesOfType(projectFileContent, "Compile");
		}

		public IEnumerable<FileProjectItem> GetProjectFiles(string projectFileContent) {
			return GetIncludes(projectFileContent, "Compile", "Content", "Resource", "None");
		}

		public IEnumerable<FileProjectItem> GetIncludes(string projectFileContent, params string[] includeTypes) {
			return includeTypes.SelectMany(includeType => GetIncludesOfType(projectFileContent, includeType));
		}
		
		public IEnumerable<FileProjectItem> GetIncludesOfType(string projectFileContent, string includeType) {
			var root = ProjectRootElement.Create(new XmlTextReader(new StringReader(projectFileContent)));
			var filePaths = root.Items.Where(element => element.ItemType == includeType).Select(element => element.Include);
			return from path in filePaths select new FileProjectItem(null, ItemType.Compile, path);			
		}



		public IEnumerable<string> GetFullPathsForCompiledFilesFromProjectFile(string projectFilePath) {
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var projectFolder = projectFilePath.ParentDirectory();
			var compiledFiles = this.GetCompiledFiles(projectFileContent);
			return from fileItem in compiledFiles select FileSystem.Combine(projectFolder, fileItem.Include); 
			
		}

		public IEnumerable<ReferenceProjectItem> GetReferences(string projectFileContent) {
			var root = ProjectRootElement.Create(new XmlTextReader(new StringReader(projectFileContent)));
			XmlNamespaceManager xmlNamespaceManager;
			var doc = LoadXml(projectFileContent, out xmlNamespaceManager);
			var assemblyElements = root.Items.Where(element => element.ItemType == "Reference");
			var assemblyReferences = assemblyElements.Select(element => CreateReferenceItem(element));
			var projectNodes = root.Items.Where(element => element.ItemType == "ProjectReference"); 
			var projectReferences = projectNodes.Select(node => CreateProjectReference(node));
			return
				assemblyReferences.Concat(projectReferences);
		}

		private ProjectReferenceProjectItem CreateProjectReference(ProjectItemElement projectNode) {
			return new ProjectReferenceProjectItem(new UnknownProject(@"C:\something", String.Empty), new MissingProject(@"C:\something", String.Empty)) {Include = projectNode.Include};
		}

		private ReferenceProjectItem CreateReferenceItem(ProjectItemElement referenceElement) {
			var hint = referenceElement.Metadata.FirstOrDefault(element => element.Name == "HintPath");
			var hintPath = hint != null ? hint.Value : null;
			return new ReferenceProjectItem(null, referenceElement.Include){HintPath = hintPath};
		}

		//private IEnumerable<XmlNode> GetIncludes(string nodeName, XmlDocument doc, XmlNamespaceManager xmlNamespaceManager) {
		//	var elements = GetElements(nodeName, doc, xmlNamespaceManager);
		//	return elements.Select(element => element.GetAttributeNode("Include"));
		//}

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