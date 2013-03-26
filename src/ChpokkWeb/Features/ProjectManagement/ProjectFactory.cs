using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Editor.Compilation;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using FubuCore.Util;
using ICSharpCode.SharpDevelop.Dom;

namespace ChpokkWeb.Features.ProjectManagement {
	public class ProjectFactory {
		private readonly ProjectParser _projectParser;
		private readonly IFileSystem _fileSystem;
		private readonly Compiler _compiler;
		private readonly ProjectContentRegistry _projectContentRegistry; //todo: _projectContentRegistry.ActivatePersistence
		private readonly Cache<string, ProjectData> _projects;
 
		public ProjectFactory(ProjectParser projectParser, IFileSystem fileSystem, Compiler compiler, ProjectContentRegistry projectContentRegistry) {
			_projectParser = projectParser;
			_fileSystem = fileSystem;
			_compiler = compiler;
			_projectContentRegistry = projectContentRegistry;
			_projects = new Cache<string, ProjectData>(key => LoadProject(key));
		}

		public ProjectData GetProjectData(string key) {
			return _projects[key];
		}

		private ProjectData LoadProject(string projectFilePath) {
			var filePaths = _projectParser.GetFullPathsForCompiledFilesFromProjectFile(projectFilePath);
			var readers = from path in filePaths where _fileSystem.FileExists(path) select new StreamReader(path) as TextReader;
			var projectContent = CreateDefaultProjectContent();
			_compiler.CompileAll(projectContent, readers);

			//references
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			foreach (var assemblyReference in _projectParser.GetReferences(projectFileContent)) { 
				var reference = GacInterop.FindBestMatchingAssemblyName(assemblyReference.Name);
				var fileName = GacInterop.FindAssemblyInNetGac(reference);
				var referencedContent = _projectContentRegistry.GetProjectContentForReference(assemblyReference.Name, fileName);
				projectContent.AddReferencedContent(referencedContent);
			}
			return new ProjectData(projectContent);
		}

		private DefaultProjectContent CreateDefaultProjectContent() {
			var projectContent = new DefaultProjectContent() { Language = LanguageProperties.CSharp };
			projectContent.AddReferencedContent(_projectContentRegistry.Mscorlib);
			return projectContent;
		}
		
	}
}