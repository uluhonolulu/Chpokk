using System.Collections.Generic;
using System.Text;
using Arractas;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class ParsingProjectFile : BaseQueryTest<ProjectWithSingleRootFileContext, IEnumerable<RepositoryItem>> {
		[Test]
		public void CanSeeTheFile() {
			Assert.AreEqual(1, Result.Count());
		}

		[Test, DependsOn("CanSeeTheFile")]
		public void CaptionIsFilename() {
			Assert.AreEqual(Context.FILE_NAME, FileItem.Name);
		}

		[Test, DependsOn("CanSeeTheFile")]
		public void CanEditTheFile() {
			var expectedFileName = FileSystem.Combine(@"src\ProjectName\Class1.cs");
			Assert.AreEqual(expectedFileName, FileItem.PathRelativeToRepositoryRoot);
			Assert.AreEqual("file", FileItem.Type);
		}

		protected RepositoryItem FileItem {
			get { return Result.First(); }
		}

		public override IEnumerable<RepositoryItem> Act() {
			var loader = Context.Container.Get<SolutionFileLoader>();
			return
				loader.CreateProjectItem(Context.SolutionFolder,
				                         new ProjectItem {Name = Context.PROJECT_NAME, Path = Context.PROJECT_PATH}, Context.RepositoryRoot).Children;
		}
	}
}
