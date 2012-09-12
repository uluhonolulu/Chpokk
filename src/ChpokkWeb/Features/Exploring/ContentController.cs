using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ChpokkWeb.Features.Editor;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core;
using HtmlTags;
using System.Linq;

namespace ChpokkWeb.Features.Exploring {
	public class ContentController {
		private IFileSystem _fileSystem;

		[NotNull]
		private readonly RepositoryManager _repositoryManager;
		public ContentController(RepositoryManager repositoryManager, IFileSystem fileSystem) {
			_repositoryManager = repositoryManager;
			_fileSystem = fileSystem;
		}


		//[UrlPattern("Project/{Name}")]
		[JsonEndpoint]
		public FileListModel GetFileList(FileListInputModel model) {
			var repositoryInfo = _repositoryManager.GetRepositoryInfo(model.Name);
			var root = new RepositoryItem{PathRelativeToRepositoryRoot = @"\"};
			var repositoryRoot = Path.Combine(model.PhysicalApplicationPath, repositoryInfo.Path);
			ImportFolder(root, repositoryRoot);
			return new FileListModel{Items = root.Children.ToArray()}; 
		}

		private void ImportFolder(RepositoryItem parent, string repositoryRoot) {
			var path = repositoryRoot.AppendPathMyWay(parent.PathRelativeToRepositoryRoot);
			foreach (var directory in Directory.GetDirectories(path).Where(directory => (new DirectoryInfo(directory).Attributes & FileAttributes.System) == 0)) {
				var item = new RepositoryItem {Name = Path.GetFileName(directory), PathRelativeToRepositoryRoot = directory.Substring(repositoryRoot.Length), Type = "folder"};
				parent.Children.Add(item);
				ImportFolder(item, repositoryRoot);
			}
			foreach (var filePath in Directory.GetFiles(path)) {
				var fileName = Path.GetFileName(filePath);
				var item = new RepositoryItem { Name = fileName, PathRelativeToRepositoryRoot = Path.Combine(parent.PathRelativeToRepositoryRoot, fileName), Type = "file" };
				parent.Children.Add(item);				
			}

		}

		//[AsymmetricJson]
		public CodeEditorModel GetContent(FileContentInputModel model) {
			var repositoryInfo = _repositoryManager.GetRepositoryInfo(model.RepositoryName); 
			var repositoryRoot = Path.Combine(model.PhysicalApplicationPath, repositoryInfo.Path);
			var filePath = repositoryRoot.AppendPathMyWay(model.RelativePath);
			return new CodeEditorModel{Content = _fileSystem.ReadStringFromFile(filePath), ProjectPath = model.ProjectPath, RepositoryName = model.RepositoryName};
		}
	}
}