using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class ParsingProjectContent : BaseQueryTest<ProjectContentWithOneRootFileContext, IEnumerable<RepositoryItem>> {
		[Test]
		public void CanSeeOneCodeFile() {
			Assert.AreEqual(1, Result.Count());
		}

		public override IEnumerable<RepositoryItem> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			return parser.GetProjectItems(Context.PROJECT_FILE_CONTENT);
		}
	}

	public class ProjectContentWithOneRootFileContext : SimpleConfiguredContext {
		public string PROJECT_FILE_CONTENT =
			@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Compile Include=""Class1.cs"" />
				  </ItemGroup>
				</Project>";
	}
}
