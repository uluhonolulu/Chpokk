using System.Collections.Generic;
using System.Linq;
using Arractas;
using ChpokkWeb.Features.Exploring;
using ICSharpCode.SharpDevelop.Project;
using MbUnit.Framework;

namespace Chpokk.Tests.Exploring.UnitTests {
	[TestFixture]
	public class ParsingAssemblyReferences : BaseQueryTest<ProjectContentWithOneBclReferenceContext, IEnumerable<string>> {
		[Test]
		public void ReturnsSingleReference() {
			var reference = Result.Single();
			//Assert.AreEqual("System.Core", reference.Name);
		}

		public override IEnumerable<string> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			return parser.GetBclReferences(ProjectContentWithOneBclReferenceContext.PROJECT_FILE_CONTENT);
		}
	}
}