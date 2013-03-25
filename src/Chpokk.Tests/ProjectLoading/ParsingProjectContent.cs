using System.Collections.Generic;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using ICSharpCode.SharpDevelop.Project;
using MbUnit.Framework;
using System.Linq;

namespace Chpokk.Tests.Exploring.UnitTests {
	[TestFixture]
	public class ParsingProjectContent : BaseQueryTest<ProjectContentWithOneRootFileContext, IEnumerable<FileProjectItem>> {

		[Test]
		public void CanSeeOneCodeFile() {
			Assert.AreEqual(1, Result.Count());
		}

		[Test, DependsOn("CanSeeOneCodeFile")]
		public void ItemNameIsFilename() {
			Assert.AreEqual(Context.FILE_NAME, FileItem.FileName);
		}

		public override IEnumerable<FileProjectItem> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			return parser.GetCompiledFiles(Context.PROJECT_FILE_CONTENT);
		}

		public FileProjectItem FileItem {
			get { return Result.First(); }
		}
	}

	public class ProjectContentWithOneRootFileContext : SimpleConfiguredContext {
		public string PROJECT_ROOT = "root";
		public readonly string FILE_NAME = "Class1.cs";
		public string PROJECT_FILE_CONTENT =
			@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Compile Include=""Class1.cs"" />
				  </ItemGroup>
				</Project>";
	}

	//public class ParsingProjectContentWithAFileInASubfolder : BaseQueryTest<ProjectContentWithOneFileInASubfolderContext, IEnumerable<RepositoryItem>> {
	//    public override IEnumerable<RepositoryItem> Act() {
	//        var parser = Context.Container.Get<ProjectParser>();
	//        return parser.GetCompiledFiles(Context.PROJECT_FILE_CONTENT, Context.PROJECT_ROOT);
	//    }
	//}

	public class ProjectContentWithOneFileInASubfolderContext : SimpleConfiguredContext {
		public string PROJECT_ROOT = "root";
		public readonly string FILE_NAME = "Class1.cs";
		public readonly string FILE_PATH = @"Subfolder\\Class1.cs";
		public string PROJECT_FILE_CONTENT =
			@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Compile Include=""Subfolder\\Class1.cs"" />
				  </ItemGroup>
				</Project>";
	}
}
