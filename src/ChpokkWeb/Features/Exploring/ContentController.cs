using System.Collections.Generic;
using System.IO;
using System.Text;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using HtmlTags;
using System.Linq;

namespace ChpokkWeb.Features.Exploring {
	public class ContentController {
		private IUrlRegistry _registry;
		public ContentController(IUrlRegistry registry) {
			_registry = registry;
		}

		

		//[UrlPattern("Project/{Name}")]
		[JsonEndpoint]
		public FileListModel GetFileList(FileListInputModel model) {
			var root = new RepositoryItem{PathRelativeToRepositoryRoot = @"\"};
			var repositoryRoot = Path.Combine(model.PhysicalApplicationPath, RepositoryInfo.Path);
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
		public string GetContent(FileContentInputModel model) {
			var repositoryRoot = Path.Combine(model.PhysicalApplicationPath, RepositoryInfo.Path);
			var filePath = repositoryRoot.AppendPathMyWay(model.RelativePath);
			return File.ReadAllText(filePath, Encoding.Default);
		}
	}
}