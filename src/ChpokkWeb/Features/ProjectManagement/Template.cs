using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

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

		//public string Path { get; private set; }

		public string ProjectFileName {
			get {
				var projectNode = _xmlDocument.SelectSingleNode("//d:Project", _namespaceManager);
				return projectNode.Attributes["File"].Value;
			}
		}

		public IEnumerable<ProjectItem> GetProjectItems() {
			return from XmlNode projectItemNode in _xmlDocument.SelectNodes("//d:ProjectItem", _namespaceManager)
			       select new ProjectItem(projectItemNode);
		} 

		public class ProjectItem {
			private readonly XmlNode _itemNode;
			public ProjectItem(XmlNode itemNode) {
				_itemNode = itemNode;
			}

			public string FileName { get { return _itemNode.InnerText; } }
			public string TargetFileName { 
				get {
					var targetFileAttribute = _itemNode.Attributes["TargetFileName"];
					return targetFileAttribute != null ? targetFileAttribute.Value : FileName;
				} 
			}
		}
	}
}