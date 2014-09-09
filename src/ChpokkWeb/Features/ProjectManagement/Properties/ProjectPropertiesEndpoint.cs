using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.Bcl;
using ChpokkWeb.Features.RepositoryManagement;
using Microsoft.Build.Construction;

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
			var projectPath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.ProjectPath);
			var project = ProjectRootElement.Open(projectPath); //TODO: load from IFileSystem and use the source instead of the RootElement
			var solutionPath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.SolutionPath);
			var output = new ProjectPropertiesModel();
			output.BclReferences.AddRange(GetBclReferences(project));
			output.PackageReferences.AddRange(_projectParser.GetPackageReferences(project.RawXml));
			output.ProjectReferences.AddRange(GetProjectReferences(project.RawXml, solutionPath));
			output.ProjectName =_projectParser.GetProjectName(project);
			return output;
		}

		IEnumerable<object> GetBclReferences(ProjectRootElement project) {
			var projectReferences = _projectParser.GetBclReferences(project).ToArray();
			return from assemblyName in _assembliesProvider.BclAssemblies
			       select new
				       {
					       Name = assemblyName,
						   Selected = projectReferences.Contains(assemblyName)
				       };
		}

		IEnumerable<object> GetProjectReferences(string projectContent, string solutionPath) {
			var projectReferences = _projectParser.GetProjectReferences(projectContent).ToArray();
			return from projectItem in _solutionParser.GetProjectItems(solutionPath)
			       select new
				       {
					       projectItem.Name, 
						   projectItem.Path,
					       Selected = projectReferences.Contains(projectItem.Name)
				       };

		}


	}

	public class ProjectPropertiesModel {
		public string ProjectName;
		public IList<object> BclReferences = new List<object>();
		public IList<string> PackageReferences = new List<string>();
		public IList<object> ProjectReferences = new List<object>();
	}

	public class ProjectPropertiesInputModel: BaseRepositoryInputModel {
		public string ProjectPath { get; set; }
		public string SolutionPath { get; set; }
	}
}