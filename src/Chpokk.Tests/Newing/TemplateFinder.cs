using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Infrastructure;
using FubuCore;
using MbUnit.Framework;
using Microsoft.Build.Construction;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.Newing {
	[TestFixture]
	public class TemplateFinder : BaseQueryTest<SimpleConfiguredContext, IList<ProjectTemplateData>> {
		[Test]
		public void CanFindSimpleNamedProject() {
			var webTemplate = Result.First(data => data.Name == "ASP.NET Web Application");
			webTemplate.Path.ShouldBe(@"CSharp\Web\1033\WebTemplate45\WebTemplate.vstemplate");
			webTemplate.DisplayPath.ShouldBe(@"CSharp\Web");
			webTemplate.RequiredFrameworkVersion.ShouldBe("4.5");
		}

		[Test]
		public void FixingTheProject() {
			var projectPath = @"D:\Projects\Chpokk\src\ChpokkWeb\ChpokkWeb.csproj";
			var project = ProjectRootElement.Open(projectPath);
			var fileSystem = Context.Container.Get<FileSystem>();
			var templateFolder = Context.Container.Get<IAppRootProvider>().AppRoot.AppendPath(@"SystemFiles\Templates\ProjectTemplates\");
			var templatePaths = fileSystem.FindFiles(templateFolder, new FileSet() { DeepSearch = true });
			var relativePaths = from path in templatePaths select path.PathRelativeTo(projectPath.ParentDirectory());
			var contentPaths = from item in project.Items select item.Include;
			var notIncluded = relativePaths.Except(contentPaths);
			Console.WriteLine("Missing");
			foreach (var path in notIncluded) {
				Console.WriteLine(path);
				//project.AddItem("Content", path);
			}
			//project.Save();
			//foreach (var item in project.Items) {
			//	if (item.Include.StartsWith(@"SystemFiles\Templates")) {
			//		var templatePath = projectPath.ParentDirectory().AppendPath(item.Include);
			//		if (!File.Exists(templatePath)) {
			//			Console.WriteLine(templatePath);
			//			item.Parent.RemoveChild(item);
			//		}
			//		else {
			//			item.ItemType = "Content";
			//		}
			//	}
			//}
			//project.Save();
		}

		public override IList<ProjectTemplateData> Act() {
			var templates = new List<ProjectTemplateData>();
			var fileSystem = Context.Container.Get<FileSystem>();
			var templateFolder = Context.Container.Get<IAppRootProvider>().AppRoot.AppendPath(@"SystemFiles\Templates\ProjectTemplates\") ;
			var templateFiles = fileSystem.FindFiles(templateFolder, new FileSet() { Include = "*.vstemplate", DeepSearch = true });
			foreach (var templateFile in templateFiles) {
				var xmlDocument = new XmlDocument();
				xmlDocument.Load(templateFile);
				var ns = new XmlNamespaceManager(xmlDocument.NameTable);
				ns.AddNamespace("d", "http://schemas.microsoft.com/developer/vstemplate/2005");
				var templateData = new ProjectTemplateData()
					{
						Name = GetNodeValue(xmlDocument, ns, "Name"),
						Path = templateFile.PathRelativeTo(templateFolder),
						RequiredFrameworkVersion = GetNodeValue(xmlDocument, ns, "RequiredFrameworkVersion")
					};
				templates.Add(templateData); 
			}

			return templates;
		}
		private static string GetNodeValue(XmlDocument document, XmlNamespaceManager ns, string name) {
			var node = document.SelectSingleNode("//d:TemplateData/d:{0}".ToFormat(name), ns);
			if (node != null && node.FirstChild != null)
				return node.FirstChild.Value;
			return null;
		}

	}


	public class ProjectTemplateData {
		public string Name { get; set; }
		public string RequiredFrameworkVersion { get; set; }
		public string Path { get; set; }
		public string DisplayPath {
			get { 
				var ignoredFolders = new[] {"1033"};
				return Path.ParentDirectory().ParentDirectory().getPathParts().Except(ignoredFolders).Join(@"\");
			}
		}
	}
}
