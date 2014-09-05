using Chpokk.Tests.Infrastructure;

namespace Chpokk.Tests.ProjectLoading {
	public class ProjectContentWithOneBclReferenceContext : SimpleConfiguredContext {
		public const string PROJECT_FILE_CONTENT =
			@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Reference Include=""System.Core""/>
				  </ItemGroup>
				</Project>";
		
	}
}
