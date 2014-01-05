﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.AddSimpleProject;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using Microsoft.Build.Construction;

namespace ChpokkWeb.Features.ProjectManagement.AddProject {
	public class AddProjectBase {
		protected ProjectParser _projectParser;
		protected RepositoryManager _repositoryManager;
		protected PackageInstaller _packageInstaller;
		protected SignalRLogger _logger;

		protected void AddPackages(AddSimpleProjectInputModel inputModel, string projectPath) {
			var targetFolder = _repositoryManager.NewGetAbsolutePathFor(inputModel.RepositoryName).AppendPath("packages");
			if (inputModel.Packages != null) {
				_logger.WriteLine("Adding package references");
				foreach (var packageId in inputModel.Packages)
					if (packageId.IsNotEmpty()) _packageInstaller.InstallPackage(packageId, projectPath, targetFolder);
			}
		}

		protected void AddProjectReferences(AddProjectInputModel inputModel, string solutionPath, ProjectRootElement rootElement) {
			if (inputModel.Projects != null) {
				foreach (var relativeReferencedPath in inputModel.Projects) {
					_logger.WriteLine("Adding a reference to {0}", relativeReferencedPath);
					var referencedPath = solutionPath.ParentDirectory().AppendPath(relativeReferencedPath);
					_projectParser.AddProjectReference(rootElement, referencedPath);
				}
			}
		}

		protected void AddBclReferences(AddSimpleProjectInputModel inputModel, ProjectRootElement rootElement) {
			if (inputModel.References != null) {
				foreach (var reference in inputModel.References) {
					_logger.WriteLine("Adding a reference to {0}", reference);
					_projectParser.AddReference(rootElement, reference);
				}
			}
		}
	}
}