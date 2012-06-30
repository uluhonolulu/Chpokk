using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class ParsingProjectContent : BaseQueryTest<ProjectContentWithOneRootFileContext, IEnumerable<FileItem>> {

		[Test]
		public void CanSeeOneCodeFile() {
			Assert.AreEqual(1, Result.Count());
		}

		[Test, DependsOn("CanSeeOneCodeFile")]
		public void ItemNameIsFilename() {
			Assert.AreEqual(Context.FILE_NAME, FileItem.Path);
		}
		
		public override IEnumerable<FileItem> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			return parser.GetCompiledFiles(Context.PROJECT_FILE_CONTENT);
		}

		public FileItem FileItem {
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
