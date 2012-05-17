using System.IO;
using System.Text;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Urls;
using HtmlTags;

namespace ChpokkWeb.Features.Repa {
	public class ContentController {
		private IUrlRegistry _registry;
		public ContentController(IUrlRegistry registry) {
			_registry = registry;
		}

		

		//[UrlPattern("Project/{Name}")]
		public HtmlTag GetFileList(FileListModel model) {
			var fileList = new HtmlTag("ul");
			var repositoryRoot = Path.Combine(model.PhysicalApplicationPath, RepositoryInfo.Path);
			foreach (var file in Directory.GetFiles(repositoryRoot)) {
				var fileName = Path.GetFileName(file);
				var relativePath = file.Substring(repositoryRoot.Length);
				fileList.Add("li")
					.Data("path", relativePath)
					.Data("name", fileName)
					.Data("type", "file")
					.Text(fileName);
			}
			return fileList;
		}

		//[AsymmetricJson]
		public string GetContent(FileContentModel model) {
			var repositoryRoot = Path.Combine(model.PhysicalApplicationPath, RepositoryInfo.Path);
			var filePath = repositoryRoot.AppendPathMyWay(model.RelativePath);
			return File.ReadAllText(filePath, Encoding.Default);
		}
	}

	public class FileContentModel {
		public string PhysicalApplicationPath { get; set; }
		public string RelativePath { get; set; }
	}
}