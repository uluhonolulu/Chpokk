using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ChpokkWeb.Infrastructure.Windows;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.ProjectTemplates {
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
			template.Path = path;
			return template;
		}

		public string ProjectFileName {
			get {
				var projectNode = _xmlDocument.SelectSingleNode("//d:Project", _namespaceManager);
				return projectNode.Attributes["File"].Value;
			}
		}

		public string Name {
			get {
				var nameNode = _xmlDocument.SelectSingleNode("//d:TemplateData/d:Name", _namespaceManager);
				return nameNode.InnerText;
			}
		}

		public string RequiredFrameworkVersion {
			get {
				var node = _xmlDocument.SelectSingleNode("//d:TemplateData/d:RequiredFrameworkVersion", _namespaceManager);
				if (node == null) {
					return null;
				}
				return node.InnerText;
			}
		}

		public string Path { get; private set; }

		public IDictionary<string, string> Replacements{
			get{
				var replacements = new Dictionary<string, string>();
				var replacementNodes = _xmlDocument.SelectNodes("//d:CustomParameter", _namespaceManager);
				foreach (var node in replacementNodes.Cast<XmlNode>()) {
					replacements.Add(node.Attributes["Name"].Value, node.Attributes["Value"].Value);
				}
				return replacements;
			}
		}

		public IEnumerable<ProjectItem> GetProjectItems() {
			var projectNode = _xmlDocument.SelectSingleNode("//d:Project", _namespaceManager);
			if (projectNode == null) {
				return Enumerable.Empty<ProjectItem>();
			}
			return GetProjectItemsFromFolder(projectNode, string.Empty, string.Empty);
		}

		public string PackagesPath{
			get{
				var packagesNode = _xmlDocument.SelectSingleNode("//d:WizardData/d:packages", _namespaceManager);
				if (packagesNode == null) {
					return null;
				}
				var keyName = packagesNode.Attributes["keyName"].Value;
				return ((string)RegistryUtils.GetRegistryValue(@"SOFTWARE\Wow6432Node\NuGet\Repository", keyName));
			}
		}

		public IEnumerable<TemplatePackages.NameAndVersion> Packages{
			get
			{
				var packageNodes = _xmlDocument.SelectNodes("//d:WizardData/d:packages/d:package", _namespaceManager);
				return from node in packageNodes.Cast<XmlNode>()
					select
						new TemplatePackages.NameAndVersion
						{
							PackageId = node.Attributes["id"].Value,
							Version = node.Attributes["version"].Value
						};

			}
		}

		public TemplatePackages TemplatePackages { get {return new TemplatePackages(){Packages = this.Packages, PackageRepositoryPath = this.PackagesPath};}}

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
				FileName = _path.AppendPath(_itemNode.InnerText) ;
			}

			public string FileName { get; set; }
			public string TargetFileName { 
				get {
					var targetFileAttribute = _itemNode.Attributes["TargetFileName"];
					var fileName = targetFileAttribute != null ? targetFileAttribute.Value : FileName;
					return _targetPath.AppendPath(fileName);
				} 
			}
		}
	}

	public class TemplatePackages {
		public string PackageRepositoryPath { get; set; }
		public IEnumerable<NameAndVersion> Packages { get; set; }

		public class NameAndVersion {
			public string PackageId { get; set; }
			public string Version { get; set; }
		}
	}
}