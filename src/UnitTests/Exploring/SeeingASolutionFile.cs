using System;
using System.IO;
using System.Linq;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Infrastructure;
using MbUnit.Framework;

namespace UnitTests.Exploring {
	[TestFixture]
	public class SeeingASolutionFile : BaseSolutionBrowserTest<EmptySlnFileContext> {
		[Test]
		public void CanSeeTheFile() {
			var names = Result.Select(item => item.Name);
			Assert.AreElementsEqual(new[]{ Path.GetFileNameWithoutExtension(Context.SolutionFileName) }, names);
		}


		[Test]
		public void WillNotBeAbleToEditIt() {
			var first = Result.First();
			Assert.AreEqual("folder", first.Type);
		}

	}

	public class EmptySlnFileContext : SingleSlnFileContext {
		public override void CreateSolutionFile([NotNull] string filePath) {
			Container.Get<SolutionFileLoader>().CreateEmptySolution(filePath);
		}
	}

}
