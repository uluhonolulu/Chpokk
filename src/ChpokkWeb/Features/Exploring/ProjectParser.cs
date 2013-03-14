using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FubuCore;

namespace ChpokkWeb.Features.Exploring {
	public class ProjectParser {
		private readonly IFileSystem _fileSystem;
		public ProjectParser(IFileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public IEnumerable<FileItem> GetCompiledFiles(string projectFileContent) {
			var doc = new XmlDocument();
			doc.LoadXml(projectFileContent);
			var xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);
			xmlNamespaceManager.AddNamespace("ms", "http://schemas.microsoft.com/developer/msbuild/2003");
			var filePaths = doc.SelectNodes("//ms:Compile/@Include", xmlNamespaceManager).Cast<XmlNode>().Select(node => node.Value);
			return from path in filePaths select new FileItem { Path = path };
		}

		public IEnumerable<string> GetFullPathsForCompiledFilesFromProjectFile(string projectFilePath) {
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var projectFolder = projectFilePath.ParentDirectory();
			var compiledFiles = this.GetCompiledFiles(projectFileContent);
			return from fileItem in compiledFiles select FileSystem.Combine(projectFolder, fileItem.Path); 
			
		}
	}

	public class FileItem {
		public string Path { get; set; }
	}
}