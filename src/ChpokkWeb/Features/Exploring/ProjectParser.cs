using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure.FileSystem;
using FubuCore;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Project;
using ChpokkWeb.Infrastructure;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Exceptions;
using NuGet;
using NuGet.Common;
using IFileSystem = FubuCore.IFileSystem;

namespace ChpokkWeb.Features.Exploring {
	public class ProjectParser {
		private readonly IFileSystem _fileSystem;
		private readonly ProjectCollection _projectCollection;
		private readonly IAppRootProvider _rootProvider;
		private readonly FileExistenceChecker _checker;
		public ProjectParser(IFileSystem fileSystem, ProjectCollection projectCollection, IAppRootProvider rootProvider, FileExistenceChecker checker) {
			_fileSystem = fileSystem;
			_projectCollection = projectCollection;
			_rootProvider = rootProvider;
			_checker = checker;
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
			var root = CreateRootElement(projectFileContent);
			var filePaths = root.Items.Where(element => element.ItemType == includeType).Select(element => element.Include);
			return from path in filePaths select new FileProjectItem(null, ItemType.Compile, path);			
		}

		private ProjectRootElement CreateRootElement(string projectFileContent) {
			return ProjectRootElement.Create(new XmlTextReader(new StringReader(projectFileContent)));
		}


		public IEnumerable<string> GetFullPathsForCompiledFilesFromProjectFile(string projectFilePath) {
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var projectFolder = projectFilePath.ParentDirectory();
			var compiledFiles = this.GetCompiledFiles(projectFileContent);
			return from fileItem in compiledFiles select FileSystem.Combine(projectFolder, fileItem.Include); 
			
		}

		//public IEnumerable<ReferenceProjectItem> GetReferences(string projectFileContent) {
		//	var root = ProjectRootElement.Create(new XmlTextReader(new StringReader(projectFileContent)));
		//	return GetReferences(root);
		//}

		//public IEnumerable<ReferenceProjectItem> GetReferences(ProjectRootElement root) {
		//	var assemblyElements = root.Items.Where(element => element.ItemType == "Reference");
		//	var assemblyReferences = assemblyElements.Select(element => CreateReferenceItem(element));
		//	var projectNodes = root.Items.Where(element => element.ItemType == "ProjectReference");
		//	var projectReferences = projectNodes.Select(node => CreateProjectReference(node));
		//	return
		//		assemblyReferences.Concat(projectReferences);
		//}

		//private ProjectReferenceProjectItem CreateProjectReference(ProjectItemElement projectNode) {
		//	return new ProjectReferenceProjectItem(new UnknownProject(@"C:\something", String.Empty), new MissingProject(@"C:\something", String.Empty)) {Include = projectNode.Include};
		//}

		//private ReferenceProjectItem CreateReferenceItem(ProjectItemElement referenceElement) {
		//	var hint = referenceElement.Metadata.FirstOrDefault(element => element.Name == "HintPath");
		//	var hintPath = hint != null ? hint.Value : null;
		//	return new ReferenceProjectItem(null, referenceElement.Include){HintPath = hintPath};
		//}

		//private IEnumerable<XmlNode> GetIncludes(string nodeName, XmlDocument doc, XmlNamespaceManager xmlNamespaceManager) {
		//	var elements = GetElements(nodeName, doc, xmlNamespaceManager);
		//	return elements.Select(element => element.GetAttributeNode("Include"));
		//}

		public ProjectRootElement CreateProject(string outputType, SupportedLanguage language, string projectPath = null, string projectName = null) {
			var rootElement = ProjectRootElement.Create();
			var targetImport =
				@"$(MSBuildToolsPath)\Microsoft.{0}.targets".ToFormat(language == SupportedLanguage.CSharp ? "CSharp" : "VisualBasic");
			rootElement.AddImport(targetImport);
			rootElement.DefaultTargets = "Build";
			rootElement.AddProperty("OutputType", outputType);
			rootElement.AddProperty("OutputPath", @"bin\Debug\");
			if (projectName != null) {
				rootElement.AddProperty("AssemblyName", projectName);
				rootElement.AddProperty("RootNamespace", projectName);
			}
			if (projectPath != null) rootElement.Save(projectPath);
			//create Program.cs
			if (outputType == "Exe") {
				CreateProgramFile(language, rootElement);
			}
			return rootElement;
		}


		public void AddProjectToSolution(string name, string solutionPath, SupportedLanguage language) {
			AddProjectToSolution(name, solutionPath, language.GetProjectGuid(), language.GetProjectExtension());
		} 

		public void AddProjectToSolution(string name, string solutionPath, string projectTypeGuid, string projectFileExtension) {
			var solutionContent = _fileSystem.ReadStringFromFile(solutionPath);
			//adding project to the project section
			//solutionContent += Environment.NewLine;
			// in fact, we have two sections: project and config
			// project section is up to a line that contains just "Global", or till the end
			// 
			var projectGuid = Guid.NewGuid();
			AddProjectSectionToSolutionContent(name, projectTypeGuid, projectFileExtension, projectGuid, ref solutionContent);
			AddGlobalSectionToSolutionContent(projectGuid, ref solutionContent);

			//solutionContent += @""
			_fileSystem.WriteStringToFile(solutionPath, solutionContent);
		}

		public void AddProjectSectionToSolutionContent(string name, string projectTypeGuid, string projectFileExtension,
		                                               Guid projectGuid, ref string solutionContent) {
			const string projectPattern = @"(.+?)(?=\r\n\s*Global\s*$)";
			var projectRegex = new Regex(projectPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
			var newProjectContent = @"
Project(""{1}"") = ""{0}"", ""{0}\{0}{3}"", ""{{{2}}}""
EndProject".ToFormat(name, projectTypeGuid, projectGuid, projectFileExtension);
			if (projectRegex.IsMatch(solutionContent))
				solutionContent = projectRegex.Replace(solutionContent, "$&" + newProjectContent);
			else solutionContent += newProjectContent;
		}

		public void AddGlobalSectionToSolutionContent(Guid projectGuid, ref string solutionContent) {
			const string globalPattern = @"(?<=GlobalSection\(ProjectConfigurationPlatforms\) = postSolution\s*)(.*?)(?=\r\n\s*EndGlobalSection)";
			var globalRegex = new Regex(globalPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);	
			var newProjectContent = @"
		{{{0}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{{0}}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{{0}}}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{{{0}}}.Release|Any CPU.Build.0 = Release|Any CPU".ToFormat(projectGuid);//Need Release|Any CPU for AppHarbor
			solutionContent = globalRegex.Replace(solutionContent, "$&" + newProjectContent);
		}

		public void CreateItem(string projectFilePath, string fileName, string fileContent) {
			var project = ProjectRootElement.Open(projectFilePath);
			CreateItem(fileName, fileContent, project);
		}

		private void CreateItem(string fileName, string fileContent, ProjectRootElement project) {
			var extension = Path.GetExtension(fileName);
			var buildAction = "Content";
			if (extension == ".cs" || extension == ".vb") buildAction = "Compile";
			project.AddItem(buildAction, fileName);
			project.Save();
			//ProjectCollection.GlobalProjectCollection.UnloadProject(project);

			var filePath = project.FullPath.ParentDirectory().AppendPath(fileName);
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


		private void CreateProgramFile(SupportedLanguage language, string projectPath) {
			var filename = language == SupportedLanguage.CSharp ? "Program.cs" : "Module1.vb";
			var fileContent = GetFileContent(filename);
			this.CreateItem(projectPath, filename, fileContent);
		}
		private void CreateProgramFile(SupportedLanguage language, ProjectRootElement rootElement) {
			if (rootElement.FullPath != null) CreateProgramFile(language, rootElement.FullPath);
		}

		private static string GetFileContent(string filename) {
			var assembly = Assembly.GetExecutingAssembly();
			var reader = new StreamReader(assembly.GetManifestResourceStream("ChpokkWeb.App_GlobalResources.FileTemplates." + filename));
			return reader.ReadToEnd();
		}

		public void AddProjectReference(ProjectRootElement targetProject, ProjectRootElement referencedProject) {
			var relativePath = referencedProject.FullPath.PathRelativeTo(targetProject.FullPath.ParentDirectory());
			var referenceItem = targetProject.AddItem("ProjectReference", relativePath);
			var nameProperty = referencedProject.Properties.FirstOrDefault(element => element.Name == "AssemblyName");
			if (nameProperty != null) referenceItem.AddMetadata("Name", nameProperty.Value);
			targetProject.Save();
		}

		public void AddProjectReference(ProjectRootElement targetProject, string referencedPath) {
			var referencedProject = ProjectRootElement.Open(referencedPath);
			AddProjectReference(targetProject, referencedProject);
		}

		public IEnumerable<string> GetProjectReferences(string projectFileContent) {
			var root = ProjectRootElement.Create(new XmlTextReader(new StringReader(projectFileContent)));
			return GetProjectReferences(root);
		}

		public IEnumerable<string> GetBclReferences(string projectFileContent) {
			return GetBclReferences(CreateRootElement(projectFileContent));
		}

		public IEnumerable<string> GetBclReferences(ProjectRootElement root) {
			return from item in root.Items
			       where item.ItemType == "Reference" && !item.HasMetadata
			       select item.Include;
		}

		//a list of relative paths
		public IEnumerable<string> GetProjectReferences(ProjectRootElement root) {
			return from item in root.Items
			       where item.ItemType == "ProjectReference"
			       select item.Include;
		}

		public IEnumerable<dynamic> GetPackageReferences(string projectPath, IEnumerable<IPackage> allPackages) {
			//preload the project with the global vars that we need
			LoadProject(projectPath);
			var projectSystem = new MSBuildProjectSystem(projectPath);
			return from package in allPackages
				   select new {
					   Name = package.Id,
					   Selected = package.AssemblyReferences.Any(reference => projectSystem.ReferenceExists(reference.Name))
				   };
		}

		public IEnumerable<string> GetFileReferences(ProjectRootElement root) {
			return from item in root.Items
				   where item.ItemType == "Reference" && 
				   item.HasMetadata &&
				   item.Metadata.Any(element => element.Name == "HintPath")
				   select item.Metadata.Single(element => element.Name == "HintPath").Value;			
		}

		private bool IsPackageReference(ProjectItemElement item) {
			return item.ItemType == "Reference" && 
					item.HasMetadata && 
					item.Metadata.Any(element => element.Name == "HintPath") &&
					item.Metadata.Single(element => element.Name == "HintPath").Value.Contains("packages");
		}

		public string GetProjectName(ProjectRootElement root) {
			var propertyElement = root.Properties.FirstOrDefault(element => element.Name == "AssemblyName");
			if (propertyElement == null) {
				propertyElement = root.Properties.FirstOrDefault(element => element.Name == "RootNamespace");
			}
			return propertyElement != null ? propertyElement.Value : null;
		}

		public string GetProjectOutputType(ProjectRootElement root) {
			var propertyElement = root.Properties.Single(element => element.Name == "OutputType");
			return propertyElement.Value;
		}

		public string GetProjectLanguage(ProjectRootElement root) {
			if (root.Imports.Any(element => element.Project.EndsWith("VisualBasic.targets"))) {
				return "VBNet";
			}
			if (root.Imports.Any(element => element.Project.EndsWith("CSharp.targets"))) {
				return "CSharp";
			}
			throw new InvalidProjectFileException("Can't determine language for " + GetProjectName(root));
		}

		public void ClearReferences(ProjectRootElement root) {
			foreach (var item in root.Items) {
				if (item.ItemType == "Reference" || item.ItemType == "ProjectReference") {
					item.Parent.RemoveChild(item);
				}
			}
		}

		public Project LoadProject(string projectFilePath) {
			var customProperties = new Dictionary<string, string>()
				{
					{"VSToolsPath", _rootProvider.AppRoot.AppendPath(@"SystemFiles\Targets")}
				};
			_checker.VerifyFileExists(projectFilePath);
			var project = _projectCollection.LoadProject(projectFilePath, customProperties, null);
			return project;
		}


		public static void UnloadProject(string projectPath) {
			//unload the project so that it's not cached
			var project = ProjectCollection.GlobalProjectCollection.GetLoadedProjects(projectPath).FirstOrDefault();
			if (project != null)
				ProjectCollection.GlobalProjectCollection.UnloadProject(project);
		}
	}

}