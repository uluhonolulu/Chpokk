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
		private ProjectParser _projectParser;
		private RepositoryManager _repositoryManager;
		public ProjectPropertiesEndpoint(BclAssembliesProvider assembliesProvider, ProjectParser projectParser, RepositoryManager repositoryManager) {
			_assembliesProvider = assembliesProvider;
			_projectParser = projectParser;
			_repositoryManager = repositoryManager;
		}

		public ProjectPropertiesModel DoIt(ProjectPropertiesInputModel model) {
			var projectPath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.ProjectPath);
			var project = ProjectRootElement.Open(projectPath);
			var output = new ProjectPropertiesModel();
			output.BclReferences.AddRange(GetBclReferences(project));
			output.PackageReferences.Add("Autofac");
			output.ProjectReferences.Add(new {Name = "OtherProject", Path = "ThePath", Selected = true});
			output.ProjectName = "ProjectName";
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
	}

	public class ProjectPropertiesModel {
		public string ProjectName;
		public IList<object> BclReferences = new List<object>();
		public IList<string> PackageReferences = new List<string>();
		public IList<object> ProjectReferences = new List<object>();
	}

	public class ProjectPropertiesInputModel: BaseRepositoryInputModel {
		public string ProjectPath { get; set; }
	}
}