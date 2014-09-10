using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.AddProject;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Project;
using Microsoft.Build.Evaluation;

namespace ChpokkWeb.Features.ProjectManagement.AddSimpleProject {
	public class AddSimpleProjectEndpoint : AddProjectBase {
		private readonly SolutionFileLoader _solutionFileLoader;
		private readonly IUrlRegistry _registry;

		public AddSimpleProjectEndpoint(ProjectParser projectParser, RepositoryManager repositoryManager, PackageInstaller packageInstaller, SignalRLogger logger, SolutionFileLoader solutionFileLoader, IUrlRegistry registry) : base(projectParser, repositoryManager, packageInstaller, logger) {
			_solutionFileLoader = solutionFileLoader;
			_registry = registry;
		}

		public AjaxContinuation DoIt(AddSimpleProjectInputModel inputModel) {
			_logger.ConnectionId = inputModel.ConnectionId; // so that we can write to the logger

			var repositoryName = inputModel.RepositoryName;
			var solutionPath = _repositoryManager.GetAbsolutePathFor(repositoryName, inputModel.PhysicalApplicationPath, repositoryName + ".sln");
			_solutionFileLoader.CreateEmptySolution(solutionPath);

			var outputType = inputModel.OutputType;
			var language = inputModel.Language;

			_projectParser.AddProjectToSolution(repositoryName, solutionPath, language);

			//create a project
			var projectPath = _repositoryManager.NewGetAbsolutePathFor(repositoryName, Path.Combine(repositoryName, repositoryName + language.GetProjectExtension()));
			var rootElement = _projectParser.CreateProject(outputType, language, projectPath, repositoryName);

			AddBclReferences(inputModel, rootElement);
			AddPackages(inputModel, projectPath);
			rootElement.Save();
			_logger.WriteLine("Project created");

			//redirect to the new repository
			var projectUrl = _registry.UrlFor(new RepositoryInputModel { RepositoryName = repositoryName });
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}


	}
}