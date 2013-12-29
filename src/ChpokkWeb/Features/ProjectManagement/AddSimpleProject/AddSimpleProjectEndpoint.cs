﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Project;
using Microsoft.Build.Evaluation;

namespace ChpokkWeb.Features.ProjectManagement.AddSimpleProject {
	public class AddSimpleProjectEndpoint {
		private readonly IFileSystem _fileSystem;
		private readonly SolutionFileLoader _solutionFileLoader;
		private readonly RepositoryManager _repositoryManager;
		private readonly ProjectParser _projectParser;
		private readonly IUrlRegistry _registry;
		private readonly PackageInstaller _packageInstaller;
		public AddSimpleProjectEndpoint(IFileSystem fileSystem, SolutionFileLoader solutionFileLoader, RepositoryManager repositoryManager, ProjectParser projectParser, IUrlRegistry registry, PackageInstaller packageInstaller) {
			_fileSystem = fileSystem;
			_solutionFileLoader = solutionFileLoader;
			_repositoryManager = repositoryManager;
			_projectParser = projectParser;
			_registry = registry;
			_packageInstaller = packageInstaller;
		}

		public AjaxContinuation DoIt(AddSimpleProjectInputModel inputModel) {
			var repositoryName = inputModel.RepositoryName;
			var solutionPath = _repositoryManager.GetAbsolutePathFor(repositoryName, inputModel.PhysicalApplicationPath, repositoryName + ".sln");
			_solutionFileLoader.CreateEmptySolution(solutionPath);

			var outputType = inputModel.OutputType;
			var language = inputModel.Language;

			_projectParser.AddProjectToSolution(repositoryName, solutionPath, language);

			//create a project
			var projectPath = _repositoryManager.NewGetAbsolutePathFor(repositoryName, Path.Combine(repositoryName, repositoryName + language.GetProjectExtension()));
			var rootElement = _projectParser.CreateProject(outputType, language, projectPath, repositoryName);

			if (inputModel.References != null)
				foreach (var reference in inputModel.References) {
					_projectParser.AddReference(rootElement, reference);
				}
			//ProjectCollection.GlobalProjectCollection.UnloadProject(rootElement);


			//install packages
			var targetFolder = _repositoryManager.GetAbsolutePathFor(repositoryName,
			                                                         inputModel.PhysicalApplicationPath).AppendPath("packages");
			if (inputModel.Packages != null) {
				foreach (var packageId in inputModel.Packages) {
					if (packageId.IsNotEmpty()) {
						//new Thread(() => {_packageInstaller.InstallPackage(packageId, projectPath, targetFolder);}).Start();
						_packageInstaller.InstallPackage(packageId, projectPath, targetFolder);
					}
				}
			}
			rootElement.Save();

			var projectUrl = _registry.UrlFor(new RepositoryInputModel { RepositoryName = repositoryName });
			return AjaxContinuation.Successful().NavigateTo(projectUrl);
		}


	}
}