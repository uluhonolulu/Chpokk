using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class SeeingASolutionFile : BaseQueryTest<SingleSolutionContext, IEnumerable<RepositoryItem>> {
		[Test]
		public void CanSeeTheFile() {
			var names = Result.Select(item => item.Name);
			Assert.AreElementsEqual(new[]{Context.FileName}, names);
		}

		[Test]
		public void ThePathIsCorrect() {
			var paths = Result.Select(item => item.PathRelativeToRepositoryRoot);
			Assert.AreElementsEqual(new[]{@"\Repka\" + Context.FileName}, paths);
		}

		[Test]
		public void WillBeAbleToEditIt() {
			var first = Result.First();
			Assert.AreEqual("file", first.Type);
		}

		public override IEnumerable<RepositoryItem> Act() {
			var controller = Context.Container.Get<SolutionContentController>();
			return controller.GetSolutions(new SolutionExplorerInputModel { Name = Context.REPO_NAME, PhysicalApplicationPath = Path.GetFullPath(@"..") }).Items;
		}
	}

	public class WhenFileIsNotASolutionOne  : BaseQueryTest<SingleFileContext, IEnumerable<RepositoryItem>> {

		[Test]
		public void FileListShouldBeEmpty() {
			Assert.IsEmpty(Result);
		}

		public override IEnumerable<RepositoryItem> Act() {
			var controller = Context.Container.Get<SolutionContentController>();
			return controller.GetSolutions(new SolutionExplorerInputModel { Name = Context.REPO_NAME, PhysicalApplicationPath = Path.GetFullPath(@"..") }).Items;
		}
	}

	public class SingleSolutionContext : RepositoryFolderContext {
		public string FileName { get; set; }
		public string FilePath { get; set; }

		public override void Create() {
			base.Create();
			FileName = Guid.NewGuid().ToString() + ".sln";
			FilePath = Path.Combine(RepositoryRoot, FileName);
			File.Create(FilePath).Dispose();			
		}

	}
}
