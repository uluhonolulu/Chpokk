using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Evaluation;

namespace ChpokkWeb.Features.ProjectManagement.AddItem {
	public class AddItemEndpoint {
		private readonly RepositoryManager _repositoryManager;
		private readonly IFileSystem _fileSystem;
		public AddItemEndpoint(RepositoryManager repositoryManager, IFileSystem fileSystem) {
			_repositoryManager = repositoryManager;
			_fileSystem = fileSystem;
		}

		public AjaxContinuation DoIt(AddItemInputModel model) {
			var repositoryInfo = _repositoryManager.GetRepositoryInfo(model.RepositoryName);
			var projectFilePath = FileSystem.Combine(model.PhysicalApplicationPath, repositoryInfo.Path,
			                              model.ProjectPathRelativeToRepositoryRoot);
			var fileName =
				model.PathRelativeToRepositoryRoot.PathRelativeTo(model.ProjectPathRelativeToRepositoryRoot.ParentDirectory());
			var project = ProjectCollection.GlobalProjectCollection.LoadProject(projectFilePath); //new Project(projectFilePath);
			project.AddItem("Compile", fileName);
			project.Save();
			ProjectCollection.GlobalProjectCollection.UnloadProject(project);

			var filePath = _repositoryManager.GetAbsolutePathFor(model.RepositoryName, model.PhysicalApplicationPath, model.PathRelativeToRepositoryRoot);
			var fileContent = string.Empty;
			_fileSystem.WriteStringToFile(filePath, fileContent);

			return AjaxContinuation.Successful();

			//project absolute path
			//file relative pth
			//file content
			//use ProjectRoot
		}
	}

	public class AddItemInputModel: BaseFileInputModel {
		public string ProjectPathRelativeToRepositoryRoot { get; set; }
	}
}