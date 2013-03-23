using System;
using System.Text;
using Chpokk.Tests.Infrastructure;
using Gallio.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Exploring.UnitTests {
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
