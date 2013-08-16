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
		readonly LanguageDetector _languageDetector;
		private readonly ProjectContentRegistry _projectContentRegistry; //todo: _projectContentRegistry.ActivatePersistence
		private readonly ProjectCache _projects;
 
		public ProjectFactory(ProjectParser projectParser, IFileSystem fileSystem, Compiler compiler, ProjectContentRegistry projectContentRegistry, LanguageDetector languageDetector, ProjectCache projects) {
			_projectParser = projectParser;
			_fileSystem = fileSystem;
			_compiler = compiler;
			_projectContentRegistry = projectContentRegistry;
			_languageDetector = languageDetector;
			_projects = projects;
			_projects.OnMissing = LoadProjectData;
		}

		public ProjectData GetProjectData(string key) {
			return _projects[key];
		}

		private ProjectData LoadProjectData(string projectFilePath) {
			return new ProjectData(LoadProject(projectFilePath));
		}

		private DefaultProjectContent LoadProject(string projectFilePath) {
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
				else {
					var displayedFileName = assemblyReference.HintPath ?? assemblyReference.Name;
					throw new FileNotFoundException(
						"Reference to '{0}' not found in project {1}".ToFormat(displayedFileName, Path.GetFileName(projectFilePath)), fileName);
				}
			}

			foreach (var projectReference in projectReferences) {
				var fileName = projectReference.FileName;
				fileName = Path.GetFullPath(Path.Combine(projectFilePath.ParentDirectory(), fileName));
				if (fileName != null && _fileSystem.FileExists(fileName)) {
					var referencedContent = GetProjectData(fileName).ProjectContent;
					projectContent.AddReferencedContent(referencedContent);
				}
				else {
					throw new FileNotFoundException(
						"Reference to '{0}' not found in project {1}".ToFormat(projectReference.FileName, Path.GetFileName(projectFilePath)), fileName);
				}
			}
			return projectContent;
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