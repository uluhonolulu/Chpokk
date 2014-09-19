using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.Bcl;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ChpokkWeb.Features.RepositoryManagement;
using Microsoft.Build.Construction;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.Properties {
	public class ProjectPropertiesEndpoint {
		private readonly BclAssembliesProvider _assembliesProvider;
		private readonly ProjectParser _projectParser;
		private readonly RepositoryManager _repositoryManager;
		private readonly SolutionParser _solutionParser;
		private readonly PackageInstaller _packageInstaller;
		private IFileSystem _fileSystem;
		public ProjectPropertiesEndpoint(BclAssembliesProvider assembliesProvider, ProjectParser projectParser, RepositoryManager repositoryManager, SolutionParser solutionParser, PackageInstaller packageInstaller, IFileSystem fileSystem) {
			_assembliesProvider = assembliesProvider;
			_projectParser = projectParser;
			_repositoryManager = repositoryManager;
			_solutionParser = solutionParser;
			_packageInstaller = packageInstaller;
			_fileSystem = fileSystem;
		}

		public ProjectPropertiesModel DoIt(ProjectPropertiesInputModel model) {
			var projectPath = model.ProjectPath.IsNotEmpty()? _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.ProjectPath) : null;
			var project = projectPath != null? ProjectRootElement.Open(projectPath): null; //TODO: load from IFileSystem and use the source instead of the RootElement
			var solutionPath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.SolutionPath);
			var output = new ProjectPropertiesModel();
			output.BclReferences.AddRange(GetBclReferences(project));
			var repositoryRoot = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName);
			if (project != null) output.PackageReferences.AddRange(GetPackages(projectPath, repositoryRoot));
			output.ProjectReferences.AddRange(GetProjectReferences(project, solutionPath));
			output.FileReferences.AddRange(GetFileReferences(project, repositoryRoot));
			if (project != null) {
				output.ProjectName =_projectParser.GetProjectName(project);
				output.ProjectType = _projectParser.GetProjectOutputType(project);
				output.Language = _projectParser.GetProjectLanguage(project);				
			}

			return output;
		}

		public IEnumerable<object> GetPackages(string projectPath, string repositoryRoot) {
			var allPackages = _packageInstaller.GetAllPackages(repositoryRoot);
			ProjectParser.UnloadProject(projectPath); //so that it is not cached in the global project collection -- might keep references
			return _projectParser.GetPackageReferences(projectPath, allPackages);
		}

		IEnumerable<object> GetBclReferences(ProjectRootElement project) {
			var projectReferences = project != null ? _projectParser.GetBclReferences(project).ToArray() : null; 
			return from assemblyName in _assembliesProvider.BclAssemblies
			       select new
				       {
					       Name = assemblyName,
						   Selected = projectReferences != null? projectReferences.Contains(assemblyName) : false
				       };
		}

		IEnumerable<object> GetProjectReferences(ProjectRootElement project, string solutionPath) {
			var projectReferences = project != null ? _projectParser.GetProjectReferences(project).ToArray() : null;
			var projectName = project != null ? _projectParser.GetProjectName(project) : null;
			return from projectItem in _solutionParser.GetProjectItems(solutionPath)
				   where projectItem.Name != projectName
			       select new
				       {
					       projectItem.Name, 
						   projectItem.Path,
					       Selected = projectReferences != null? projectReferences.Contains(projectItem.Name) : false
				       };

		}

		IEnumerable<object> GetFileReferences(ProjectRootElement project, string repositoryRoot) {
			var assemblyPaths = _fileSystem.FindFiles(repositoryRoot, new FileSet() {Include = "*.dll"});
			var referencedPaths = _projectParser.GetFileReferences(project);
			var absoluteReferencedPaths = from referencedPath in referencedPaths
										  select Path.GetFullPath(project.FullPath.ParentDirectory().AppendPath(referencedPath));
			return from assemblyPath in assemblyPaths
			       select new
				       {
					       Name = assemblyPath.PathRelativeTo(repositoryRoot),
						   Selected = absoluteReferencedPaths.Contains(assemblyPath)
				       };
		}


	}

	public class ProjectPropertiesModel {
		public string ProjectName;
		public string ProjectType;
		public string Language;
		public IList<object> BclReferences = new List<object>();
		public IList<object> PackageReferences = new List<object>();
		public IList<object> ProjectReferences = new List<object>();
		public IList<object> FileReferences = new List<object>();
	}

	public class ProjectPropertiesInputModel: BaseRepositoryInputModel {
		public string ProjectPath { get; set; }
		public string SolutionPath { get; set; }
	}
}