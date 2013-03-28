using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Arractas;
using ChpokkWeb.Features.ProjectManagement;
using ICSharpCode.SharpDevelop.Dom;
using MbUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = MbUnit.Framework.Assert;

namespace Chpokk.Tests.ProjectLoading {
	[TestFixture]
	public class AddingLocalReferenceToAProject : BaseQueryTest<ProjectFileWithLocalReferenceContext, IProjectContent> {
		[Test]
		public void ProjectHasALocalReference() {
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
