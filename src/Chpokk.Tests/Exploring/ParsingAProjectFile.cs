using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arractas;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Exploring {
	[TestFixture, DependsOn(typeof(ParsingTheSolutionFile))]
	public class ParsingAProjectFile : BaseQueryTest<SolutionAndProjectWithSingleFileContext, IEnumerable<RepositoryItem>> {
		[Test]
		public void ShouldSeeOneFile() {
			var files = ProjectItem.Children;
			Assert.AreEqual(1, files.Count);
		}

		public override IEnumerable<RepositoryItem> Act() {
			var controller = Context.Container.Get<SolutionContentController>();
			return
				controller.GetSolutions(new SolutionExplorerInputModel
				                        {Name = Context.REPO_NAME, PhysicalApplicationPath = Path.GetFullPath(@"..")}).Items;
		}

		public RepositoryItem SolutionItem {
			get { return Result.First(); }
		}

		public RepositoryItem ProjectItem {
			get { return SolutionItem.Children.First(); }
		}	
	}

	public class SolutionAndProjectWithSingleFileContext : SingleSolutionContext {
			const string projectFileContent = 
				@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Compile Include=""Class1.cs"" />
				  </ItemGroup>
				</Project>";
		public override void Create() {
			base.Create();
			var projectFilePath = FileSystem.Combine(this.RepositoryRoot, this.PROJECT_PATH);
			var fileSystem = Container.Get<FileSystem>();
			fileSystem.WriteStringToFile(projectFilePath, projectFileContent);
		}
	}
}
