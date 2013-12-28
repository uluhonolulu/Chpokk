using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;

namespace ChpokkWeb.Features.ProjectManagement.References.OtherProjects {
	public class OtherProjectsEndPoint {
		private SolutionParser _solutionParser;
		private RepositoryManager _repositoryManager;
		public OtherProjectsEndPoint(SolutionParser solutionParser, RepositoryManager repositoryManager) {
			_solutionParser = solutionParser;
			_repositoryManager = repositoryManager;
		}

		public OtherProjectsModel DoIt(OtherProjectsInputModel model) {
			var solutionPath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.SolutionPath);
			var projectItems = _solutionParser.GetProjectItems(solutionPath);
			return new OtherProjectsModel() { Projects = projectItems };
		}
	}
}