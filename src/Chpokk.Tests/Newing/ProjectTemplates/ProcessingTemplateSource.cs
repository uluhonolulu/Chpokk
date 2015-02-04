using System.Collections.Generic;
using ChpokkWeb.Features.ProjectManagement;
using MbUnit.Framework;
using Shouldly;

namespace Chpokk.Tests.Newing.ProjectTemplates {
	[TestFixture]
	public class ProcessingTemplateSource {


		[Test]
		public void CanProcessIfStatementsInATemplate() {
			const string source = "$if$ ($targetframeworkversion$ >= 4.0)<supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v$targetframeworkversion$\" />$endif$$if$ ($targetframeworkversion$ < 4.0)<supportedRuntime version=\"v2.0.50727\" />$endif$";
			var replacements = new Dictionary<string, string>() {{"$targetframeworkversion$", "4.0"}};
			string result = new TemplateTransformer().Evaluate(source, replacements);
			result.ShouldBe("<supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.0\" />");
		}

	}
}
