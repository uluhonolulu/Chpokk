using System.Collections.Generic;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using ICSharpCode.SharpDevelop.Project;
using MbUnit.Framework;
using System.Linq;
using Shouldly;
using FubuCore;

namespace Chpokk.Tests.ProjectLoading {
	[TestFixture]
	public class ParsingProjectReferences : BaseQueryTest<ProjectContentWithOneProjectReferenceContext, IEnumerable<string>> {
		[Test]
		public void ReturnsSingleReference() {
			var projectName = Result.Single();
			projectName.ShouldBe(Context.PROJECT_NAME);
		}

		public override IEnumerable<string> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			return parser.GetProjectReferences(Context.PROJECT_FILE_CONTENT);
		}
	}

	public class ProjectContentWithOneProjectReferenceContext : SimpleConfiguredContext {
		public string PROJECT_NAME = "ChpokkWeb";
		public string PROJECT_FILE_CONTENT;

		public ProjectContentWithOneProjectReferenceContext() {
			PROJECT_FILE_CONTENT = string.Format(
				@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<ProjectReference Include=""..\{0}\{0}.csproj"">
					  <Project>{{244CBBEC-847E-4E0D-8857-05BB34878174}}</Project>
					  <Name>{0}</Name>
					</ProjectReference>
				  </ItemGroup>
				</Project>", PROJECT_NAME);
		}
	}
}
