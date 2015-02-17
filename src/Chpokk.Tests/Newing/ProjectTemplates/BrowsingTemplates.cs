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
			var endpoint = Context.Container.Get<TemplateListEndpoint>();
			return endpoint.DoIt().Templates.ToList();
		}
		private static string GetNodeValue(XmlDocument document, XmlNamespaceManager ns, string name) {
			var node = document.SelectSingleNode("//d:TemplateData/d:{0}".ToFormat(name), ns);
			if (node != null && node.FirstChild != null)
				return node.FirstChild.Value;
			return null;
		}

	}



}
