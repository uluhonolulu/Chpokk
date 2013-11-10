using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CSharpBinding;
using ChpokkWeb.Features.Compilation;
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
			//_projects.OnMissing = LoadProjectData;
		}




		public class DummyProjectChangeWatcher : IProjectChangeWatcher {
			public void Dispose() { }
			public void Enable() { }
			public void Disable() { }
			public void Rename(string newFileName) { }
		}
	}
}