using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using NuGet;
using Shouldly;

namespace Chpokk.Tests.ProjectLoading {
	[TestFixture]
	public class CheckingPackagesWhenBclOnly : BaseQueryTest<ProjectFileWithOneBclReferenceContext, IEnumerable<object>> {
		[Test]
		public void ShouldBeNoPackageReferences() {
			Result.ShouldBeEmpty();
		}

		public override IEnumerable<object> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			return parser.GetPackageReferences(Context.ProjectPath, Enumerable.Empty<IPackage>());
		}
	}

	public class ProjectFileWithOneBclReferenceContext: ProjectFileContext {
		public override string ProjectFileContent {
			get { return ProjectContentWithOneBclReferenceContext.PROJECT_FILE_CONTENT; }
		}
	}
}
