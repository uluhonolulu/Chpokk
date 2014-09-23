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
	public class ParsingAProjectFile : BaseQueryTest<SolutionAndProjectFileWithSingleEntryContext, IEnumerable<RepositoryItem>> {
		[Test]
		public void ShouldSeeOneFile() {
			var files = ProjectItem.Children;
			Assert.AreEqual(1, files.Count);
		}

		public override IEnumerable<RepositoryItem> Act() {
			var controller = Context.Container.Get<SolutionContentEndpoint>();
			return
				controller.GetSolutions(new SolutionExplorerInputModel
				                        {RepositoryName = Context.REPO_NAME}).Items;
		}

		public RepositoryItem SolutionItem {
			get { return Result.First(); }
		}

		public RepositoryItem ProjectItem {
			get { return SolutionItem.Children.First(); }
		}	
	}

	public class SolutionAndProjectFileWithSingleEntryContext : SingleSolutionWithProjectFileContext {
		public const string CODEFILE_NAME = "Class1.cs";

		public string projectFileContent = 
				string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Compile Include=""{0}"" />
				  </ItemGroup>
				</Project>", CODEFILE_NAME);

		protected override string GetProjectFileContent() {
			return projectFileContent;
		}

		private string _projectFilePath;

		public override void Create() {
			base.Create();
			_projectFilePath = FileSystem.Combine(SolutionFolder, this.PROJECT_PATH);
			ProjectFolder = _projectFilePath.ParentDirectory();
			var fileSystem = Container.Get<FileSystem>();
			Console.WriteLine("Writing project to " + _projectFilePath);
			fileSystem.WriteStringToFile(_projectFilePath, GetProjectFileContent());
		}
	}
}
