using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arractas;
using ChpokkWeb.Features.ProjectManagement;
using Gallio.Framework;
using ICSharpCode.SharpDevelop.Dom;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.ProjectLoading {
	[TestFixture]
	public class AddsBclReferenceToAProject: BaseQueryTest<ProjectFileWithBCLReferenceContext, IProjectContent> {
		[Test]
		public void ProjectHasABclReference() {
			var references = Result.ReferencedContents;
			var referencesNotCountingMscorlib = from reference in references
			                                    where reference.AssemblyName != "mscorlib"
			                                    select reference;
			Assert.AreEqual(1, referencesNotCountingMscorlib.Count()); // 
		}

		public override IProjectContent Act() {
			var factory = Context.Container.Get<ProjectFactory>();
			return factory.GetProjectData(Context.ProjectPath).ProjectContent;
		}
	}
}
