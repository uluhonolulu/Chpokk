using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement {
	public class Template {
		private  XmlDocument _xmlDocument;
		private  XmlNamespaceManager _namespaceManager;

		public Template(string xml) {
			_xmlDocument = new XmlDocument();
			_xmlDocument.LoadXml(xml);
			_namespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
			_namespaceManager.AddNamespace("d", "http://schemas.microsoft.com/developer/vstemplate/2005");
		}

		private Template() {}

		public static Template LoadTemplate(string path) {
			var template = new Template {_xmlDocument = new XmlDocument()};
			template._xmlDocument.Load(path);
			template._namespaceManager = new XmlNamespaceManager(template._xmlDocument.NameTable);
			template._namespaceManager.AddNamespace("d", "http://schemas.microsoft.com/developer/vstemplate/2005");
			return template;
		}

		public string ProjectFileName {
			get {
				var projectNode = _xmlDocument.SelectSingleNode("//d:Project", _namespaceManager);
				return projectNode.Attributes["File"].Value;
			}
		}

		public IEnumerable<ProjectItem> GetProjectItems() {
			var projectNode = _xmlDocument.SelectSingleNode("//d:Project", _namespaceManager);
			return GetProjectItemsFromFolder(projectNode, string.Empty, string.Empty);
			var projectItems = from XmlNode projectItemNode in projectNode.SelectNodes("d:ProjectItem", _namespaceManager)
			                   select new ProjectItem(projectItemNode, string.Empty, string.Empty);
			foreach (XmlNode folderNode in projectNode.SelectNodes("d:Folder", _namespaceManager)) {
				var subfolderItems = from XmlNode projectItemNode in folderNode.SelectNodes("d:ProjectItem", _namespaceManager)
									 select new ProjectItem(projectItemNode, folderNode.Attributes["Name"].Value, folderNode.Attributes["TargetFolderName"].Value);
				projectItems = projectItems.Concat(subfolderItems);
			}
			return projectItems;
		}

		private IEnumerable<ProjectItem> GetProjectItemsFromFolder(XmlNode folderNode, string parentPath, string parentTargetPath) {
			var localPath = folderNode.Attributes["Name"] != null? folderNode.Attributes["Name"].Value : string.Empty;
			var path = parentPath.AppendPath(localPath);
			var localTargetPath = folderNode.Attributes["TargetFolderName"] != null? folderNode.Attributes["TargetFolderName"].Value : string.Empty;
			var targetPath = parentTargetPath.AppendPath(localTargetPath);
			var projectItems = from XmlNode projectItemNode in folderNode.SelectNodes("d:ProjectItem", _namespaceManager)
									select new ProjectItem(projectItemNode, path, targetPath);
			foreach (XmlNode subfolderNode in folderNode.SelectNodes("d:Folder", _namespaceManager)) {
				var subfolderItems = GetProjectItemsFromFolder(subfolderNode, path, targetPath);
				projectItems = projectItems.Concat(subfolderItems);
			}
			return projectItems;
		}

		public class ProjectItem {
			private readonly XmlNode _itemNode;
			private readonly string _path;
			private readonly string _targetPath;
			public ProjectItem(XmlNode itemNode, string path, string targetPath) {
				_itemNode = itemNode;
				_path = path;
				_targetPath = targetPath;
			}

			public string FileName { get { return _path.AppendPath(_itemNode.InnerText) ; } }
			public string TargetFileName { 
				get {
					var targetFileAttribute = _itemNode.Attributes["TargetFileName"];
					var fileName = targetFileAttribute != null ? targetFileAttribute.Value : FileName;
					return _targetPath.AppendPath(fileName);
				} 
			}
		}
	}
}