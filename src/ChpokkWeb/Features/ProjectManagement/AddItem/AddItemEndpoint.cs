using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace ChpokkWeb.Features.ProjectManagement.AddItem {
	public class AddItemEndpoint {
		private readonly RepositoryManager _repositoryManager;
		private readonly IFileSystem _fileSystem;
		private ProjectParser _projectParser;
		public AddItemEndpoint(RepositoryManager repositoryManager, IFileSystem fileSystem, ProjectParser projectParser) {
			_repositoryManager = repositoryManager;
			_fileSystem = fileSystem;
			_projectParser = projectParser;
		}

		public AjaxContinuation DoIt(AddItemInputModel model) {
			var projectFilePath = _repositoryManager.GetAbsolutePathFor(model.RepositoryName, model.PhysicalApplicationPath,
			                                                            model.ProjectPath);
			var fileName =
				model.PathRelativeToRepositoryRoot.PathRelativeTo(model.ProjectPath.ParentDirectory());
			_projectParser.CreateItem(projectFilePath, fileName, string.Empty);

			return AjaxContinuation.Successful();
		}


	}

	public class AddItemInputModel: BaseFileInputModel {
		public string ProjectPath { get; set; }
	}
}