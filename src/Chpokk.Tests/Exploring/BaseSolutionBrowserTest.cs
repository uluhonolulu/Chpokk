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
			var controller = Context.Container.Get<SolutionContentEndpoint>();
			return controller.GetSolutions(new SolutionExplorerInputModel { RepositoryName = Context.REPO_NAME}).Items;
		}
	}

	public abstract class SingleSlnFileContext : RepositoryFolderContext {
		public string SolutionFileName { get; set; }
		public string SolutionName { get; set; }
		public string SolutionPath { get; set; }
		public string SolutionFolder { get; set; }
		public string RelativeSolutionPath { get; set; }

		public abstract void CreateSolutionFile(string filePath);

		public override void Create() {
			base.Create();
			SolutionName = Guid.NewGuid().ToString();
			SolutionFileName = SolutionName + ".sln";
			SolutionFolder = FileSystem.Combine(RepositoryRoot, "src");
			RelativeSolutionPath = "src".AppendPath(SolutionFileName);
			SolutionPath = FileSystem.Combine(SolutionFolder, SolutionFileName);
			Console.WriteLine("Writing solution to " + SolutionPath);
			CreateSolutionFile(SolutionPath);
		}
	}
}
