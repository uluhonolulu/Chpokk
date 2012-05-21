using System.Collections.Generic;
using System.IO;
using System.Text;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Urls;
using HtmlTags;
using System.Linq;

namespace ChpokkWeb.Features.Repa {
	public class ContentController {
		private IUrlRegistry _registry;
		public ContentController(IUrlRegistry registry) {
			_registry = registry;
		}

		

		//[UrlPattern("Project/{Name}")]
		public FileListModel GetFileList(FileListInputModel model) {
			var root = new RepositoryItem();
			var repositoryRoot = Path.Combine(model.PhysicalApplicationPath, RepositoryInfo.Path);
			foreach (var directory in Directory.GetDirectories(repositoryRoot)) {
				var item = new RepositoryItem {Name = Path.GetFileName(directory), PathRelativeToRepositoryRoot = directory.Substring(repositoryRoot.Length)};
				root.Children.Add(item);
				ImportFolder(item);
			}
			foreach (var file in Directory.GetFiles(repositoryRoot)) {
				root.Children.Add(new RepositoryItem{Name	= Path.GetFileName(file), PathRelativeToRepositoryRoot = file.Substring(repositoryRoot.Length)});
			}
			return new FileListModel{Items = new[]{root}}; 
		}

		private void ImportFolder(RepositoryItem parent) {
			var path = @"F:\Projects\Fubu\Chpokk\src\ChpokkWeb\Repka\Subfolder";
			var filePath = Directory.GetFiles(path).First();
			var fileName = Path.GetFileName(filePath);
			var item = new RepositoryItem { Name = fileName, PathRelativeToRepositoryRoot = Path.Combine(parent.PathRelativeToRepositoryRoot, fileName) };
			parent.Children.Add(item);
		}

		//[AsymmetricJson]
		public string GetContent(FileContentInputModel model) {
			var repositoryRoot = Path.Combine(model.PhysicalApplicationPath, RepositoryInfo.Path);
			var filePath = repositoryRoot.AppendPathMyWay(model.RelativePath);
			return File.ReadAllText(filePath, Encoding.Default);
		}
	}
}