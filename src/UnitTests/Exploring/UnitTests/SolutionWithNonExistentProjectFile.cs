using Arractas;
using ChpokkWeb.Features.Exploring;
using MbUnit.Framework;
using UnitTests.Infrastructure;

namespace UnitTests.Exploring.UnitTests {
	[TestFixture]
	public class SolutionWithNonExistentProjectFile : BaseQueryTest<SimpleConfiguredContext, RepositoryItem> {
		[Test]
		public void ReturnsNull() {
			Assert.IsNull(Result);
		}

		public override RepositoryItem Act() {
			var solutionLoader = Context.Container.Get<SolutionFileLoader>();
			return solutionLoader.CreateProjectItem(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\BlogSamples\SubclassingBinder",
													new ProjectItem { Name = string.Empty, Path = "I don't exist" }, 
													@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\BlogSamples");
		}
	}
}
