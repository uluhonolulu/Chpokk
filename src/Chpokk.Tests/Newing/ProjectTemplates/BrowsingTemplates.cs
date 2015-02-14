using System.Collections.Generic;
using System.Xml;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.ProjectManagement.ProjectTemplates;
using ChpokkWeb.Infrastructure;
using FubuCore;
using MbUnit.Framework;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.Newing.ProjectTemplates {
	[TestFixture]
	public class BrowsingTemplates : BaseQueryTest<SimpleConfiguredContext, IList<ProjectTemplateData>> {
		[Test]
		public void CanFindSimpleNamedProject() {
			var webTemplate = Result.First(data => data.Name == "ASP.NET Empty Web Application");
			webTemplate.Path.ShouldBe(@"CSharp\Web\Version2012\1033\EmptyWebApplicationProject40\EmptyWebApplicationProject40.vstemplate");
			webTemplate.DisplayPath.ShouldBe(@"CSharp\Web");
			webTemplate.RequiredFrameworkVersion.ShouldBe("4.0");
		}


		public override IList<ProjectTemplateData> Act() {
			var templates = new List<ProjectTemplateData>();
			var fileSystem = Context.Container.Get<FileSystem>();
			var templateFolder = Context.Container.Get<IAppRootProvider>().AppRoot.AppendPath(@"SystemFiles\Templates\ProjectTemplates\") ;
			var templateFiles = fileSystem.FindFiles(templateFolder, new FileSet() { Include = "*.vstemplate", DeepSearch = true });
			foreach (var templateFile in templateFiles) {
				var template = Template.LoadTemplate(templateFile);
				var templateData = new ProjectTemplateData()
					{
						Name = template.Name,
						Path = templateFile.PathRelativeTo(templateFolder),
						RequiredFrameworkVersion = template.RequiredFrameworkVersion
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



}
