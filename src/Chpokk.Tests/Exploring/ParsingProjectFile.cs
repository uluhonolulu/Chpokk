using System;
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

		public override IEnumerable<RepositoryItem> Act() {
			var loader = Context.Container.Get<SolutionFileLoader>();
			return
				loader.CreateProjectItem(Context.SolutionFolder,
				                         new ProjectItem {Name = Context.PROJECT_NAME, Path = Context.PROJECT_PATH}).Children;
		}
	}

	public class ProjectWithSingleRootFileContext : RepositoryFolderContext {
		public readonly string SOLUTION_FOLDER = "src";
		//public string PROJECT_ROOT = "root";
		public readonly string PROJECT_NAME = "ProjectName";
		public readonly string PROJECT_PATH = @"ProjectName\ProjectName.csproj";
		public readonly string FILE_NAME = "Class1.cs";
		public string PROJECT_FILE_CONTENT =
			@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Compile Include=""Class1.cs"" />
				  </ItemGroup>
				</Project>";

		public string SolutionFolder { get; set; }

		public override void Create() {
			base.Create();
			SolutionFolder = FileSystem.Combine(RepositoryRoot, SOLUTION_FOLDER);
			var projectPath = FileSystem.Combine(SolutionFolder, PROJECT_PATH);
			Console.WriteLine("Writing to " + projectPath);
			Container.Get<IFileSystem>().WriteStringToFile(projectPath, PROJECT_FILE_CONTENT);
		}
	}
}
