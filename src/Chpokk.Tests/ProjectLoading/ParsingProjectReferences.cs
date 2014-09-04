using System.Collections.Generic;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using ICSharpCode.SharpDevelop.Project;
using MbUnit.Framework;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.ProjectLoading {
	//TODO: use either MS.Build.Construction.ProjectItemElement
	[TestFixture]
	public class ParsingProjectReferences : BaseQueryTest<ProjectContentWithOneProjectReferenceContext, IEnumerable<string>> {
		[Test]
		public void ReturnsSingleReference() {
			var projectName = Result.Single();
			projectName.ShouldBe("ChpokkWeb");
		}

		public override IEnumerable<string> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			return parser.GetProjectReferences(ProjectContentWithOneProjectReferenceContext.PROJECT_FILE_CONTENT);
		}
	}

	public class ProjectContentWithOneProjectReferenceContext : SimpleConfiguredContext {
		public const string PROJECT_FILE_CONTENT =
			@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<ProjectReference Include=""..\ChpokkWeb\ChpokkWeb.csproj"">
					  <Project>{244CBBEC-847E-4E0D-8857-05BB34878174}</Project>
					  <Name>ChpokkWeb</Name>
					</ProjectReference>
				  </ItemGroup>
				</Project>";
		
	}
}
