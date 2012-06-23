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
			Assert.AreElementsEqual(new[]{Context.FileName}, Result.Select(item => item.Name));
		}

		public override IEnumerable<RepositoryItem> Act() {
			var controller = Context.Container.Get<SolutionContentController>();
			return controller.GetSolutions(new SolutionExplorerInputModel());
		}
	}

	public class SingleSolutionContext : RepositoryFolderContext {
		public string FileName { get; set; }
		public string FilePath { get; set; }

		public override void Create() {
			base.Create();
			FileName = Guid.NewGuid().ToString() + ".sln";
			FilePath = Path.Combine(RepositoryRoot, FileName);
			Console.WriteLine(FilePath);
			File.Create(FilePath).Dispose();			
		}

	}
}
