using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CSharpBinding;
using ChpokkWeb.Features.Editor.Compilation;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.LanguageSupport;
using FubuCore;
using FubuCore.Util;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Project;

namespace ChpokkWeb.Features.ProjectManagement {
	public class ProjectFactory {
		private readonly ProjectParser _projectParser;
		private readonly IFileSystem _fileSystem;
		private readonly Compiler _compiler;
		LanguageDetector _languageDetector;
		private readonly ProjectContentRegistry _projectContentRegistry; //todo: _projectContentRegistry.ActivatePersistence
		private readonly Cache<string, ProjectData> _projects;
 
		public ProjectFactory(ProjectParser projectParser, IFileSystem fileSystem, Compiler compiler, ProjectContentRegistry projectContentRegistry, LanguageDetector languageDetector) {
			_projectParser = projectParser;
			_fileSystem = fileSystem;
			_compiler = compiler;
			_projectContentRegistry = projectContentRegistry;
			_languageDetector = languageDetector;
			_projects = new Cache<string, ProjectData>(key => LoadProject(key));
		}

		public ProjectData GetProjectData(string key) {
			return _projects[key];
		}

		private ProjectData LoadProject(string projectFilePath) {
			var filePaths = _projectParser.GetFullPathsForCompiledFilesFromProjectFile(projectFilePath);
			var readers = from path in filePaths where _fileSystem.FileExists(path) select new StreamReader(path) as TextReader;
			var language = _languageDetector.GetLanguage(projectFilePath);
			var projectContent = CreateDefaultProjectContent(_languageDetector.GetLanguageProperties(projectFilePath));
			_compiler.CompileAll(projectContent, readers, language);

			//references
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var references = _projectParser.GetReferences(projectFileContent).ToArray();
			var projectReferences = references.OfType<ProjectReferenceProjectItem>();
			var assemblyReferences = references.Except(projectReferences);
			foreach (var assemblyReference in assemblyReferences) { 
				var fileName = GetAssemblyFileName(assemblyReference, projectFilePath.ParentDirectory());
				if (fileName != null && _fileSystem.FileExists(fileName)) {
					var referencedContent = _projectContentRegistry.GetProjectContentForReference(assemblyReference.Name, fileName);
					projectContent.AddReferencedContent(referencedContent);	
				}
				else throw new FileNotFoundException("Reference to '{0}' not found in project {1}".ToFormat(fileName, Path.GetFileName(projectFilePath)), fileName);
			}

			foreach (var projectReference in projectReferences) {
				var fileName = projectReference.FileName;
				fileName = Path.GetFullPath(Path.Combine(projectFilePath.ParentDirectory(), fileName));
				if (fileName != null && _fileSystem.FileExists(fileName)) {
					var solution = new Solution(new ProjectFactory.DummyProjectChangeWatcher()) { };
					fileName =
						@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu\Chpokk-SampleSol\src\ConsoleApplication1\ConsoleApplication1.csproj";
					var loadInfo = new ProjectLoadInformation(solution, fileName, String.Empty);
					// in order to load it properly, we need to load the C# addin from D:\Projects\OSS\SharpDevelop\samples\SharpSnippetCompiler\SharpSnippetCompiler\bin\AddIns\CSharpBinding\CSharpBinding.addin
					// like this: AddinTree.Load(new List{path}, new List)
					//maybe also snippetcomp for doozers
					AddInTree.Load(new List<string> { @"D:\Projects\OSS\SharpDevelop\samples\SharpSnippetCompiler\SharpSnippetCompiler\bin\AddIns\CSharpBinding\CSharpBinding.addin", @"D:\Projects\OSS\SharpDevelop\samples\SharpSnippetCompiler\SharpSnippetCompiler\bin\AddIns\SharpSnippetCompiler.addin" }, new List<string>());
					var bindings = AddInTree.BuildItems<ProjectBindingDescriptor>("/SharpDevelop/Workbench/ProjectBindings", null, false);
					//newProject = binding.LoadProject(loadInformation);
					var projectBinding = bindings[0].Binding;
					projectBinding = new CSharpProjectBinding();
					IProject newProject = projectBinding.LoadProject(loadInfo);
					var referencedContent = newProject.CreateProjectContent();
					projectContent.AddReferencedContent(referencedContent);
				}
				else throw new FileNotFoundException("Reference to '{0}' not found in project {1}".ToFormat(fileName, Path.GetFileName(projectFilePath)), fileName);
			}
			return new ProjectData(projectContent);
		}

		private static string GetAssemblyFileName(ReferenceProjectItem assemblyReference, string projectFolder) {
			if (assemblyReference.HintPath.IsNotEmpty()) {
				return Path.GetFullPath(FileSystem.Combine(projectFolder, assemblyReference.HintPath));
			}
			else {
				var reference = GacInterop.FindBestMatchingAssemblyName(assemblyReference.Name);
				if (reference == null) {
					return null;
				}
				return GacInterop.FindAssemblyInNetGac(reference);				
			}
		}

		private DefaultProjectContent CreateDefaultProjectContent(LanguageProperties language) {
			var projectContent = new DefaultProjectContent() { Language = language };
			projectContent.AddReferencedContent(_projectContentRegistry.Mscorlib);
			return projectContent;
		}


		public class DummyProjectChangeWatcher : IProjectChangeWatcher {
			public void Dispose() { }
			public void Enable() { }
			public void Disable() { }
			public void Rename(string newFileName) { }
		}
	}
}