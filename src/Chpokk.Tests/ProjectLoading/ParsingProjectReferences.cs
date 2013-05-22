using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using ICSharpCode.SharpDevelop.Project;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;

namespace Chpokk.Tests.Exploring.UnitTests {
	[TestFixture, Ignore("Project references are not implemented")]
	public class ParsingProjectReferences : BaseQueryTest<ProjectContentWithOneProjectReferenceContext, IEnumerable<ReferenceProjectItem>> {
		[Test]
		public void ReturnsSingleReference() {
			var reference = Result.Single();
			Assert.IsInstanceOfType<ProjectReferenceProjectItem>(reference);
		}

		public override IEnumerable<ReferenceProjectItem> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			return parser.GetReferences(ProjectContentWithOneProjectReferenceContext.PROJECT_FILE_CONTENT);
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
