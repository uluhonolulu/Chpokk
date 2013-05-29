using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Editor.Compilation;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.LanguageSupport;
using FubuCore;
using FubuCore.Util;
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
			var language = _languageDetector.GetLanguage(filePaths.First());
			var projectContent = CreateDefaultProjectContent(_languageDetector.GetLanguageProperties(filePaths.First()));
			_compiler.CompileAll(projectContent, readers, language);

			//references
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			foreach (var assemblyReference in _projectParser.GetReferences(projectFileContent)) { 
				var fileName = GetAssemblyFileName(assemblyReference, projectFilePath.ParentDirectory());
				if (fileName != null && _fileSystem.FileExists(fileName)) {
					var referencedContent = _projectContentRegistry.GetProjectContentForReference(assemblyReference.Name, fileName);
					projectContent.AddReferencedContent(referencedContent);	
				}
				//else throw smth
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
		
	}
}