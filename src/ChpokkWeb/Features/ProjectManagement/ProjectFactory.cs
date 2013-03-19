using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Editor.Compilation;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using ICSharpCode.SharpDevelop.Dom;

namespace ChpokkWeb.Features.ProjectManagement {
	public class ProjectFactory {
		private readonly ProjectParser _projectParser;
		private readonly IFileSystem _fileSystem;
		private readonly Compiler _compiler;
		private ProjectContentRegistry _projectContentRegistry;
		public ProjectFactory(ProjectParser projectParser, IFileSystem fileSystem, Compiler compiler, ProjectContentRegistry projectContentRegistry) {
			_projectParser = projectParser;
			_fileSystem = fileSystem;
			_compiler = compiler;
			_projectContentRegistry = projectContentRegistry;
		}

		public ProjectData GetProjectData(string key) {
			var filePaths = _projectParser.GetFullPathsForCompiledFilesFromProjectFile(key);
			var readers = from path in filePaths where _fileSystem.FileExists(path) select new StreamReader(path) as TextReader;
			var projectContent = CreateDefaultProjectContent();
			_compiler.CompileAll(projectContent, readers);
			return new ProjectData(projectContent);
		}

		private IProjectContent CreateDefaultProjectContent() {
			var projectContent = new DefaultProjectContent() { Language = LanguageProperties.CSharp };
			projectContent.AddReferencedContent(_projectContentRegistry.Mscorlib);
			return projectContent;
		}
	}
}