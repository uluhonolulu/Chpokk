using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Exploring {
	public abstract class BaseSolutionBrowserTest<TContext> : BaseQueryTest<TContext, IEnumerable<RepositoryItem>> where TContext : SingleSlnFileContext, new() {

		public override IEnumerable<RepositoryItem> Act() {
			var controller = Context.Container.Get<SolutionContentController>();
			return controller.GetSolutions(new SolutionExplorerInputModel { Name = Context.REPO_NAME, PhysicalApplicationPath = Path.GetFullPath(@"..") }).Items;
		}
	}

	public abstract class SingleSlnFileContext : RepositoryFolderContext {
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public string SolutionFolder { get; set; }

		public abstract void CreateSolutionFile(string filePath);

		public override void Create() {
			base.Create();
			FileName = Guid.NewGuid().ToString() + ".sln";
			SolutionFolder = FileSystem.Combine(RepositoryRoot, "src");
			FilePath = FileSystem.Combine(SolutionFolder, FileName);
			Console.WriteLine("Writing solution to " + FilePath);
			CreateSolutionFile(FilePath);
		}
	}
}
