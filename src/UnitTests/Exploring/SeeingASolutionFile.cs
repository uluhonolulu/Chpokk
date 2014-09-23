using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Infrastructure;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class SeeingASolutionFile : BaseSolutionBrowserTest<EmptySlnFileContext> {
		[Test]
		public void CanSeeTheFile() {
			var names = Result.Select(item => item.Name);
			Assert.AreElementsEqual(new[]{Context.SolutionFileName}, names);
		}


		[Test]
		public void WillNotBeAbleToEditIt() {
			var first = Result.First();
			Assert.AreEqual("folder", first.Type);
		}

	}

	public class WhenFileIsNotASolutionOne  : BaseCommandTest<SingleFileContext> {

		[Test]
		public void ShouldThrowAnException() {
			var controller = Context.Container.Get<SolutionContentEndpoint>();
			Assert.Throws<Exception>(() => controller.GetSolutions(new SolutionExplorerInputModel { RepositoryName = Context.REPO_NAME})) ;
		}

		public override void Act() {
		}
	}

	public class EmptySlnFileContext : SingleSlnFileContext {
		public override void CreateSolutionFile([NotNull] string filePath) {
			Container.Get<SolutionFileLoader>().CreateEmptySolution(filePath);
		}
	}

}
