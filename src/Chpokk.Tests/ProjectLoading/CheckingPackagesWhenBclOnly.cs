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
	public class CheckingPackagesWhenBclOnly : BaseQueryTest<ProjectContentWithOneBclReferenceContext, IEnumerable<object>> {
		[Test]
		public void ShouldBeNoPackageReferences() {
			Result.ShouldBeEmpty();
		}

		public override IEnumerable<object> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			//TODO: this actually needs a saved project file
			return parser.GetPackageReferences(ProjectContentWithOneBclReferenceContext.PROJECT_FILE_CONTENT);
		}
	}
}
