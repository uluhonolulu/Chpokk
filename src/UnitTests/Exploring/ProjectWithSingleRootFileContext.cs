using System;
using FubuCore;

namespace Chpokk.Tests.Exploring {
	public class ProjectWithSingleRootFileContext : ProjectFileContext {
		//public string PROJECT_ROOT = "root";

		public override string ProjectFileContent {
			get { return @"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Compile Include=""Class1.cs"" />
				  </ItemGroup>
				</Project>"; }
		}
	}
}