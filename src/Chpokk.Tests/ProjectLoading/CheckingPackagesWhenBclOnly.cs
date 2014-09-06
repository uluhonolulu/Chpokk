using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.ProjectLoading {
	[TestFixture]
	public class CheckingPackagesWhenBclOnly: BaseQueryTest<ProjectContentWithOneBclReferenceContext, IEnumerable<string>> {
		[Test]
		public void ShouldBeNoPackageReferences() {
			Result.ShouldBeEmpty();
		}

		public override IEnumerable<string> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			return parser.GetPackageReferences(ProjectContentWithOneBclReferenceContext.PROJECT_FILE_CONTENT);
		}
	}
}
