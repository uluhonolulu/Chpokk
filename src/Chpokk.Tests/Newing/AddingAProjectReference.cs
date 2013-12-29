using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using ICSharpCode.NRefactory;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Construction;
using FubuCore;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.Newing {
	[TestFixture]
	public class AddingAProjectReference : BaseCommandTest<SolutionWithTwoProjectsContext> {
		[Test]
		public void TheProjectHasToHaveAProjectReference() {
			var projectParser = Context.Container.Get<ProjectParser>();
			var references = projectParser.GetReferences(Context.NewProject);
			references.Count().ShouldBe(1);
		}

		public override void Act() {
			var existingProject = ProjectRootElement.Open(Context.ProjectFilePath);
			var projectParser = Context.Container.Get<ProjectParser>();
			projectParser.AddProjectReference(Context.NewProject, existingProject);
		}
	}
}
