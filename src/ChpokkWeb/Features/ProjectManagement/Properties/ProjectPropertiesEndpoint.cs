using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.Bcl;
using ChpokkWeb.Features.RepositoryManagement;
using Microsoft.Build.Construction;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.Properties {
	public class ProjectPropertiesEndpoint {
		private readonly BclAssembliesProvider _assembliesProvider;
		private readonly ProjectParser _projectParser;
		private readonly RepositoryManager _repositoryManager;
		private readonly SolutionParser _solutionParser;
		public ProjectPropertiesEndpoint(BclAssembliesProvider assembliesProvider, ProjectParser projectParser, RepositoryManager repositoryManager, SolutionParser solutionParser) {
			_assembliesProvider = assembliesProvider;
			_projectParser = projectParser;
			_repositoryManager = repositoryManager;
			_solutionParser = solutionParser;
		}

		public ProjectPropertiesModel DoIt(ProjectPropertiesInputModel model) {
			var projectPath = model.ProjectPath.IsNotEmpty()? _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.ProjectPath) : null;
			var project = projectPath != null? ProjectRootElement.Open(projectPath): null; //TODO: load from IFileSystem and use the source instead of the RootElement
			var solutionPath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.SolutionPath);
			var output = new ProjectPropertiesModel();
			output.BclReferences.AddRange(GetBclReferences(project));
			if (project != null) output.PackageReferences.AddRange(_projectParser.GetPackageReferences(project.RawXml));
			output.ProjectReferences.AddRange(GetProjectReferences(project, solutionPath));
			if (project != null) {
				output.ProjectName =_projectParser.GetProjectName(project);
				output.ProjectType = _projectParser.GetProjectOutputType(project);
				output.Language = _projectParser.GetProjectLanguage(project);				
			}

			return output;
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


	}

	public class ProjectPropertiesModel {
		public string ProjectName;
		public string ProjectType;
		public string Language;
		public IList<object> BclReferences = new List<object>();
		public IList<string> PackageReferences = new List<string>();
		public IList<object> ProjectReferences = new List<object>();
	}

	public class ProjectPropertiesInputModel: BaseRepositoryInputModel {
		public string ProjectPath { get; set; }
		public string SolutionPath { get; set; }
	}
}