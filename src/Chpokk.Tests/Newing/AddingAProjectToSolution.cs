using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.Newing {
	[TestFixture]
	public class AddingAProjectToSolution : BaseCommandTest<EmptySlnFileContext> {
		[Test]
		public void SolutionShouldContainAProject() {
			var solutionPath = Context.SolutionPath;
			var projectItems = Context.Container.Get<SolutionParser>().GetProjectItems(solutionPath);
			projectItems.Count().ShouldBe(1);
		}

		public override void Act() {
			
		}
	}
}
